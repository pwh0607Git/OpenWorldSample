using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;
    private Animator animator;

    public int monsterCurHP;

    private bool canTakeDamage = true;
    private bool isDown = false;

    //플레이어와의 상호작용용 플래그
    private bool isAttackingTarget;
    private bool isDetectingTarget = false;
    private bool isChaseingTarget;

    private Transform attackTarget;
    private Vector3 originalPosition;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        originalPosition = transform.position;
        attackTarget = null;
    }

    private void Update()
    {
        //공격 대상을 발견했을 때.
        if (attackTarget != null)
        {
            HandlePlayerDetection();
        }
        else
        {
            ReturnToOriginPosition();
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                //monsterController.ChangeState(MonsterAnimState.Walk);
                isWaiting = false;
                MoveToRandomPosition();
            }
        }
    }

    //이동
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    public float waitingTime = 2.0f;            //특정 지점으로 이동으 2초뒤에 이동한다.

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        randomDirection += transform.position;

        //NavMesh를 통해 몬스터의 이동범위에서 가장 가까운 유효 위치로 이동시킬것.
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, monsterData.movingAreaRedius, NavMesh.AllAreas))
        {
            // 3. 유효한 위치가 발견되면 `NavMeshAgent`의 목적지를 해당 위치로 설정.
            //agent.destination = hit.position;
        
        }
    }

    //캐릭터 인식 범위
    //공격 범위는 따로 설정하기.
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    //캐릭터가 추적범위에 들어온 상태이다.
    private void HandlePlayerDetection()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        if (distanceToTarget <= monsterData.attackableRadius)
        {
            //타겟이 공격 범위에 들어와있는 경우.
            MonsterAttackHandler();
        }
        else
        {
            //타겟이 인식범위에는 들어왔지만, 공격 범위는 벗어난 경우.
            if (attackTarget != null)
            {
                ChasePlayer();
            }
        }
    }

    private void ReturnToOriginPosition()
    {
        if (isAttackingTarget) return;

        MoveToward(originalPosition);
    }

    private void ChasePlayer()
    {
        if (isAttackingTarget) return;

        Vector3 targetDirection = attackTarget.position - transform.position;
        Quaternion targetAngle = Quaternion.LookRotation(attackTarget.position);

        //사이에 벽이 있는지 확인하기
        if (ExistingObject(targetDirection, targetAngle)) {
            isDetectingTarget = false;
            return;
        }
        else
        {
            isDetectingTarget = true;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.deltaTime * monsterData.moveSpeed);
        transform.LookAt(attackTarget);
        MoveToward(attackTarget.position);
    }

    bool ExistingObject(Vector3 direction, Quaternion angle)
    {
        if(Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            //중간에 벽이 있다면...
            return true;
        }
        return false;
    }

    private void MoveToward(Vector3 destination)
    {
        if (!isAttackingTarget)
        {
            animator.SetBool("Walk", true);
            Vector3 direction = (destination - transform.position).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * monsterData.moveSpeed);
            }
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * monsterData.moveSpeed);
        }
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    public void TakeDamage(int damage)
    {
        if (isDown || !canTakeDamage)
        {
            return;
        }
        StartCoroutine(Coroutine_TakenDamage(damage));
        monsterUI.ShowDamage(damage);
    }

    //****** 피데미지 출력
    GameObject damageTextPrefab;

    void makeDamageEffect(int damage)
    {
        //데미지 출력 메서드
        GameObject damageTextInstance = Instantiate(damageTextPrefab);
    }

    float noDamageTime = 0.4f;
    private bool isMonsterAttackCoolDown = false;          
    private float monsterAttackCooldownTime = 2f;         

    private IEnumerator Coroutine_TakenDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);

        yield return new WaitForSeconds(noDamageTime);
        canTakeDamage = true;
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
        Debug.Log("공격 쿨타임 종료!");
    }

    public void MonsterAttackHandler()
    {
        //몬스터가 플레이어를 인지한 상태이고 따로 공격도 하지 않은 상태에서만 공격 실시.
        if (isDetectingTarget && !isAttackingTarget && !isMonsterAttackCoolDown)
        {
            isAttackingTarget = true;
            StartCoroutine(Coroutine_MonsterAttack());
        }
    }

    private IEnumerator Coroutine_MonsterAttack()
    {
        animator.SetBool("Walk", false);
        animator.SetTrigger("Attack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        yield return new WaitForSeconds(animationDuration);

        //공격 범위에 Player가 존재하는지...
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, monsterData.attackDamageRadius);

        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                //Player에게 대미지 주기
                Debug.Log("Monster hit Player!!");
            }
        }
        isAttackingTarget = false;

        //공격 쿨다운 설정하기.
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    public void Down()
    {
        isDown = true;

        //이후 몬스터의 애니메이션 명 조정.
        animator.SetTrigger("Down");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        Invoke("OnDownMonster", 1.5f);
        
        //전리품 출력.
        MonsterLootHandler loots = transform.GetComponentInChildren<MonsterLootHandler>();

        if(loots != null)
        {
            loots.ShootLoots();
        }
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }
}