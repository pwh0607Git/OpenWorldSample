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

    //�÷��̾���� ��ȣ�ۿ�� �÷���
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
        //���� ����� �߰����� ��.
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

    //�̵�
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    public float waitingTime = 2.0f;            //Ư�� �������� �̵��� 2�ʵڿ� �̵��Ѵ�.

    private void MoveToRandomPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        randomDirection += transform.position;

        //NavMesh�� ���� ������ �̵��������� ���� ����� ��ȿ ��ġ�� �̵���ų��.
        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, monsterData.movingAreaRedius, NavMesh.AllAreas))
        {
            // 3. ��ȿ�� ��ġ�� �߰ߵǸ� `NavMeshAgent`�� �������� �ش� ��ġ�� ����.
            //agent.destination = hit.position;
        
        }
    }

    //ĳ���� �ν� ����
    //���� ������ ���� �����ϱ�.
    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    //ĳ���Ͱ� ���������� ���� �����̴�.
    private void HandlePlayerDetection()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        if (distanceToTarget <= monsterData.attackableRadius)
        {
            //Ÿ���� ���� ������ �����ִ� ���.
            MonsterAttackHandler();
        }
        else
        {
            //Ÿ���� �νĹ������� ��������, ���� ������ ��� ���.
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

        //���̿� ���� �ִ��� Ȯ���ϱ�
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
            //�߰��� ���� �ִٸ�...
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

    //****** �ǵ����� ���
    GameObject damageTextPrefab;

    void makeDamageEffect(int damage)
    {
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
    public void MonsterAttackHandler()
    {
        //���Ͱ� �÷��̾ ������ �����̰� ���� ���ݵ� ���� ���� ���¿����� ���� �ǽ�.
        if (isDetectingTarget && !isAttackingTarget && !isMonsterAttackCoolDown)
        {
            StartMonsterAttack(); 
        }
    }

    public void StartMonsterAttack(){
        isAttackingTarget = true;
        animator.SetBool("Walk", false);
        animator.SetTrigger("Attack");
    }

    //애니메이션 이벤트.
    public void PerformAttack(){
         Collider[] hitTargets = Physics.OverlapSphere(transform.position, monsterData.attackDamageRadius);

        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log($"{gameObject.name} hit Player!!");
                target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(10);
            }
        }
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
        isAttackingTarget = false;
    }


    public void Down()
    {
        isDown = true;

        //���� ������ �ִϸ��̼� �� ����.
        animator.SetTrigger("Down");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        Invoke("OnDownMonster", 1.5f);
        
        //����ǰ ���.
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