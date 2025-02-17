using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ForestGolemController : Boss
{    
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;              //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;

    void LookAtTarget(){
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    #region Short-Range
    [SerializeField] float shortAttackRange;
    
    bool InShortRangeTarget(){
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceToTarget <= shortAttackRange;
    }
    
    public float knockBackPower = 100f;
    void KnockBackTarget(){
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;                                                                            //테스트로 수평으로만 수행하도록 설정.
        direction += Vector3.up*2;
        target.GetComponentInChildren<PlayerController>().ApplyKnockBack(direction * knockBackPower);
    }
    #endregion

    #region Attack Animaton Event
    public int currentPhase = 1;
    [SerializeField] int throwingCount = 0;
    //Attack Throw
    public void StartThrowRock(){
        StartAttackEvent();
        float additiveScale = currentPhase == 1 ? 1f: 5f;
        
        takenRock = Instantiate(rockPrefab, takenRockPosition);
        takenRock.transform.localScale *= additiveScale;
    }
    public void PerformThrowRock(){
        float speed = currentPhase == 1 ? 3f : 1.5f;
        takenRock.GetComponent<ThrowAbleStone>().Throw((target.transform.position + Random.insideUnitSphere).FlattenY(), speed);
        takenRock.transform.SetParent(null);
        takenRock = null;
    }

    public void EndThrowRock(){
        throwingCount++;
        EndAttackEvent();
    }

    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float jumpDuration = 2f;
    [SerializeField] float jumpAttackRange = 8f;
    public void StartJumpAttack(){
        StartAttackEvent();
        transform.DOJump(target.transform.position, jumpHeight, 1, jumpDuration);
        throwingCount = 0;
    }

    public void PerformJumpAttack(){
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        if(distanceToTarget <= jumpAttackRange){                    // 나중에  y축간의 거리 연산 조건 추가하기
            target.GetComponent<PlayerController>().PlayerTakeDamage(10);            
        }
    }
    
    public void EndJumpAttack(){
        EndAttackEvent();
    }

    public void StartShortRangeAttack(){
        StartAttackEvent();
    }

    public void PerformShortRangeAttack(){
        Vector3 attackOffset = transform.localPosition + Vector3.up + transform.forward*5f + transform.right/2;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, availableDamageZone);
        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Attack Punch To Player!!!");
                target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(10);             //TestPower
                // KnockBackTarget();
            }
        }
    }
    public void EndShortRangeAttack(){
        EndAttackEvent();
    }

    //점프 어택 실행 조건
    public bool ConditionJumpAttack(){
        return ConditionLongRange() && throwingCount >= testThrowingCount;
    }

    private void StartAttackEvent() {
        isAttacking = true;
        Debug.Log("Attack Start");
    }
    private void EndAttackEvent(){
        isAttacking = false;
        Debug.Log("Attack End");
    }
    #endregion

    [SerializeField] float availableDamageZone;
    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 3, shortAttackRange);
        
        //TakeDamageArea
        Vector3 attackOffset = transform.localPosition + Vector3.up + transform.forward*5f + transform.right/2;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOffset, availableDamageZone);

        //JumpAttackArea
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, jumpAttackRange);
    }
    
    #region BT
    [SerializeField] private BTNode rootNode;
    void Start()
    {
        InitBossMonster();
    }

    void InitBossMonster(){
        target = GameObject.FindWithTag("Player");
        TryGetComponent(out animator);
        TryGetComponent(out controller);

        originalPosition = transform.position;

        isPerformingStage = false;
        isAttacking = false;
        isDown = false;

        InitBT();
    }
    
    void Update()
    {   
        if(isPerformingStage){
            rootNode.Evaluate();
        }
    }
    void InitBT(){
        rootNode = new Sequence(new List<BTNode>{  // 전체에 쿨다운 적용
            new Selector(new List<BTNode>{
                new Sequence(new List<BTNode>{
                    new ConditionNode(ConditionShortRange),
                    new LookAtTargetNode(transform, target.transform, animator),
                    new ActionNode(AttackShortRange),
                }),
                new Sequence(new List<BTNode>{
                    new ConditionNode(ConditionJumpAttack),
                    new ActionNode(AttackJump),
                }),
                new Sequence(new List<BTNode>{
                    new ConditionNode(ConditionLongRange),
                    new LookAtTargetNode(transform, target.transform, animator),
                    new ActionNode(AttackLongRange),
                }),
                new ActionNode(Idle),
            }),
            new IntervalNode(5f)  // 공격 후 무조건 3초 대기
        });
    }

    void Idle(){
        Debug.Log("Idle 상태...");
    }

    bool ConditionShortRange(){
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceToTarget <= shortAttackRange;
    }

    [SerializeField] int testThrowingCount;
    bool ConditionLongRange(){
        return isPerformingStage; //&& throwingCount < testThrowingCount;                       // 어짜피 범위 밖으로 나가면 isPerforming은 false가 되도록 설정한다.
    }

    void AttackShortRange(){
        if(isAttacking) return;
        animator.SetTrigger("Short-RangeAttack");
    }

    void AttackLongRange(){
        if(isAttacking) return;
        animator.SetTrigger("Long-RangeAttack");
    }

    void AttackJump(){
        if(isAttacking) return;
        animator.SetTrigger("JumpAttack");
    }
    #endregion
}