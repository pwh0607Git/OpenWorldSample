using UnityEngine;

public class BasicBossState{
    public int HP;
    public float moveSpeed;
    public BasicBossState(int HP, float moveSpeed){
        this.HP = HP;
        this.moveSpeed = moveSpeed;
    }
}

public class ForestGolemController : MonoBehaviour
{
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;  //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;
    public Animator animator;
    [SerializeField] bool isAttacking;
    
    [SerializeField] float ThrowAttackTime = 2.0f;
    [SerializeField] float timer;
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        isAttacking = false;
        TryGetComponent(out animator);
    
        timer = 0.0f;
    }

   void Update()
    {   
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
        // HandleAttack();
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

    public void Punch(){        //펀지는 player를 넉백 시킨다.
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, shortAttackRange);
        foreach(var target in  hitTargets)
        {
            if (target.CompareTag("Player"))
            {
                Debug.Log("Attack Punch To Player!!!");
                target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(10);         //TestPower
            }
        }
    }

    void KnockBackTarget(){
        //넉백 방향 계산하기
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;            //테스트로 수평으로만 수행하도록 설정.
        target.GetComponentInChildren<PlayerController>().ApplyKnockBack(direction);
    }
    #endregion

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

    [SerializeField] float availableDamageZone;
    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 3, shortAttackRange);
        
        //TakeDamageArea
        Vector3 attackOffset = transform.localPosition + Vector3.up * 2.3f + transform.forward * 4 + transform.right * -1.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOffset, availableDamageZone);
    }
}