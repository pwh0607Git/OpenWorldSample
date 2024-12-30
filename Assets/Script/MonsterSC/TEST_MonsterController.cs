using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
    private bool isAttacking;
    private bool isDetecting;
    private bool isChaseing;

    private Transform attackTarget;
    private float lastAttackTime = -Mathf.Infinity;
    private Vector3 originalPosition;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        originalPosition = transform.position;
        attackTarget = null;
        isAttacking = false;
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

        if (distanceToTarget <= monsterData.attackDamageRadius)
        {
            //타겟이 공격 범위에 들어와있는 경우.
            AttackHandler();
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
        if (isAttacking) return;

        MoveToward(originalPosition);

        if (Vector3.Distance(transform.position, originalPosition) < 0.1f)
        {
            animator.SetTrigger("Idle");
        }
    }

    private void ChasePlayer()
    {
        if (isAttacking) return;

        Vector3 dir = attackTarget.position - transform.position;
        transform.LookAt(attackTarget);
        Quaternion targetAngle = Quaternion.LookRotation(attackTarget.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * monsterData.moveSpeed);
        MoveToward(attackTarget.position);
    }

    private bool IsMovingArea()
    {
        float distance = Vector3.Distance(transform.position, originalPosition);

        return distance < monsterData.movingArea;
    }

    private void MoveToward(Vector3 destination)
    {
        //현재 공격중이라면 무시...
        if (!isAttacking)
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

    private bool IsAttackTarget(float distanceToTarget)
    {
        //현재 타겟이 공격 가능 범위에 들어 왔는가.
        return(distanceToTarget <= monsterData.attackDamageRadius);
    }

    //UI 세팅
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
        StartCoroutine(HandleDamage(damage));
    }

    float noDamageTime = 0.4f;

    private IEnumerator HandleDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);

        yield return new WaitForSeconds(noDamageTime);
        canTakeDamage = true;
    }

    public void AttackHandler()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            StartCoroutine(MonsterAttack());
        }
    }

    private float monsterAttackCooldown = 2f;

    private IEnumerator MonsterAttack()
    {
        animator.SetBool("Walk", false);
        animator.SetTrigger("Attack");
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animationDuration = stateInfo.length;

        Debug.Log($"Animation Duration : {animationDuration}");
        yield return new WaitForSeconds(animationDuration);

        //공격 범위에 Player가 존재하는지...
        Collider[] hitTargets = Physics.OverlapSphere(transform.position, monsterData.attackRadius);

        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                //Player에게 대미지 주기
                Debug.Log("Monster hit Player!!");
            }
        }
        isAttacking = false;
        
        //공격 쿨다운 설정하기.
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