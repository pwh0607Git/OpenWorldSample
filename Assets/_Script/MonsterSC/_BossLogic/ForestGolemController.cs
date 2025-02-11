using System.Collections.Generic;
using UnityEngine;


public class ForestGolemController : Boss
{    
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;  //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;
    public bool isPerformingStage;

    [SerializeField] bool isAttacking;
    [SerializeField] float ThrowAttackTime = 2.0f;
    [SerializeField] float timer;
    [SerializeField] Vector3 originalPosition;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        isPerformingStage = false;
        isAttacking = false;
        TryGetComponent(out animator);
        TryGetComponent(out controller);
        originalPosition = transform.position;
        timer = 0.0f;
        animator.SetTrigger("StartStage");
        InitBT();
    }
  
    void StartBossStage(){
        animator.SetTrigger("Start");
        isPerformingStage = true;
    }

    void EndBossStage(){
        isPerformingStage = false;
        ReturnOriginalPosition();
    }

    void ReturnOriginalPosition(){
        controller.Move(originalPosition * 10f * Time.deltaTime);
    }

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

    public void Punch(){                //펀치는 player를 넉백 시킨다.
        Vector3 attackOffset = transform.localPosition + Vector3.up * 2.3f + transform.forward * 4 + transform.right * -1f;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, availableDamageZone);
        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Attack Punch To Player!!!");
                target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(10);         //TestPower
                KnockBackTarget();
            }
        }
    }
    
    public float knockBackPower = 100f;
    void KnockBackTarget(){
        //넉백 방향 계산하기
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;                                                                //테스트로 수평으로만 수행하도록 설정.
        direction += Vector3.up;
        target.GetComponentInChildren<PlayerController>().ApplyKnockBack(direction * knockBackPower);
    }
    #endregion

    int currentPhase = 1;
    #region Attack Animaton Event
    public void StartThrowRock(){
        Debug.Log("돌 만들기");
        if(currentPhase == 1){
            takenRock = Instantiate(rockPrefab, takenRockPosition);
        }
        else if(currentPhase == 2)
        {

        }
    }
    public void ThrowRock(){
        if(currentPhase == 1){
            Debug.Log("돌 던지기!");
            takenRock.GetComponent<ThrowAbleStone>().Throw((target.transform.position + Random.insideUnitSphere).FlattenY());
            takenRock.transform.SetParent(null);
            takenRock = null;
        }
        if(currentPhase == 2){
        }
    }

    public void EndAttackEvent(){
        isAttacking = false;
        timer = 0.0f;
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

    #region BT
    [SerializeField] private BTNode rootNode;

   void Update()
    {   
        rootNode.Evaluate();
    }

    bool isWaiting = false;
    public bool testFlag = true;
    void InitBT(){
        rootNode = new Selector(new List<BTNode>{
            new Sequence(new List<BTNode>{
                new ConditionNode(OnEnterShortRange),
                new IntervalDebugNode("OnEnterShortRange"),
                new ActionNode(AttackShortRange),
                new WaitNode(5f),
            }),
            new Sequence(new List<BTNode>{
                new ConditionNode(OnEnterLongRange),
                new IntervalDebugNode("OnEnterLongRange"),
                new ActionNode(AttackLongRange), 
                new WaitNode(5f),
            }),
            new Sequence(new List<BTNode>{
                new IntervalDebugNode("Idle..."),   
                new ActionNode(Idle)
            }),               
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
        return true;            // 어짜피 범위 밖으로 나가면 isPerforming은 false가 되도록 설정한다.
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