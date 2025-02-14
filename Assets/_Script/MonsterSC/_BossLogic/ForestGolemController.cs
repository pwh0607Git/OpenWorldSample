using System.Collections.Generic;
using UnityEngine;

public class ForestGolemController : Boss
{    
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;              //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;
    [SerializeField] bool isAttacking;
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

    public void Punch(){
        Vector3 attackOffset = transform.localPosition + Vector3.up * 2.3f + transform.forward * 4 + transform.right * -1f;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, availableDamageZone);
        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Attack Punch To Player!!!");
                target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(10);             //TestPower
                KnockBackTarget();
            }
        }
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
    public void StartThrowRock(){
        float additiveScale = currentPhase == 1 ? 1f: 5f;
        
        takenRock = Instantiate(rockPrefab, takenRockPosition);
        takenRock.transform.localScale *= additiveScale;
    }
    public void ThrowRock(){
        float speed = currentPhase == 1 ? 3f : 1.5f;
        takenRock.GetComponent<ThrowAbleStone>().Throw((target.transform.position + Random.insideUnitSphere).FlattenY(), speed);
        takenRock.transform.SetParent(null);
        takenRock = null;
    }

    public void EndAttackEvent(){
        isAttacking = false;
    }
    #endregion

    [SerializeField] float availableDamageZone;
    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 3, shortAttackRange);
        
        //TakeDamageArea
        Vector3 attackOffset = transform.localPosition + Vector3.up * 2.3f + transform.forward * 4 + transform.right * -1f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOffset, availableDamageZone);
    }

    #region TakeDamage

    #endregion

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
        rootNode = new Selector(new List<BTNode>{
            new Sequence(new List<BTNode>{
                new ConditionNode(OnEnterShortRange),
                new ActionNode(AttackShortRange),
                new WaitNode(5f),
            }),
            new Sequence(new List<BTNode>{
                new ConditionNode(OnEnterLongRange),
                new ActionNode(AttackLongRange), 
                new WaitNode(5f),
            }),
            new ActionNode(Idle),               
        });
    }

    void Idle(){
        Debug.Log("Idle 상태...");
    }

    bool OnEnterShortRange(){
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceToTarget <= shortAttackRange;
    }
    bool OnEnterLongRange(){
        return isPerformingStage;                       // 어짜피 범위 밖으로 나가면 isPerforming은 false가 되도록 설정한다.
    }

    void AttackShortRange(){
        if(isAttacking) return;
        animator.SetTrigger("Short-RangeAttack");
    }

    void AttackLongRange(){
        if(isAttacking) return;
        animator.SetTrigger("Long-RangeAttack");
    }
    #endregion
}