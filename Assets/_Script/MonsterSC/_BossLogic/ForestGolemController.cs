using System;
using UnityEngine;


public class ForestGolemController : Boss
{    
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;  //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;
    bool isPerformingStage = false;

    [SerializeField] bool isAttacking;
    [SerializeField] float ThrowAttackTime = 2.0f;
    [SerializeField] float timer;    
    private CharacterController controller;
    [SerializeField] Vector3 originalPosition;

    void Start()
    {
        target = GameObject.FindWithTag("Player");
        isAttacking = false;
        TryGetComponent(out animator);
        originalPosition = transform.position;
        timer = 0.0f;

        animator.SetTrigger("StartStage");
    }

   void Update()
    {   
        if(Input.GetKeyDown(KeyCode.Space)) Down();
        if(isPerformingStage) return;

        if(InShortRangeTarget()){
            if(isAttacking) return;
            animator.SetTrigger("Short-RangeAttack");
        }

        if(!isAttacking)
        {
            LookAtTarget();
            timer += Time.deltaTime;
        }
        if(timer >= ThrowAttackTime){
            animator.SetTrigger("Long-RangeAttack");
        }
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

    #region Attack Animaton Event
    public void StartThrowRock(){
        Debug.Log("돌 만들기");
        takenRock = Instantiate(rockPrefab, takenRockPosition);
    }

    public void ThrowRock(){
        Debug.Log("돌 던지기!");
        takenRock.GetComponent<ThrowAbleStone>().Throw();
        takenRock.transform.SetParent(null);
        takenRock = null;
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
}