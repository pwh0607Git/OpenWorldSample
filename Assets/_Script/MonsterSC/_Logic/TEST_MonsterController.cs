using System.Collections;
using UnityEngine;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;
    private Animator animator;

    public int monsterCurHP;

    private bool canTakeDamage = true;
    private bool isDown = false;

    private bool isAttackingTarget;
    private bool isDetectingTarget = false;

    private Transform attackTarget;
    private Vector3 originalPosition;

    private Vector3 nextDestination;
    private void Start()
    {
        monsterCurHP = monsterData.HP;
        attackTarget = null;
        TryGetComponent(out animator);
        FixPosition();
        loots = transform.GetComponentInChildren<MonsterLootHandler>().gameObject;

        SetNextDestination();
    }

    void FixPosition(){
        GetComponentInChildren<ObjectSpawnInitController>().SetOntheFloor();
        originalPosition = GetComponentInChildren<ObjectSpawnInitController>().originalPosition;
    }

    private void Update()
    {
        //Move...
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
                SetNextDestination();
            }
        }

        //위치 도착시.
        if(transform.position == nextDestination){
            if(!isWaiting){
                isWaiting = true;
                waitTimer = waitingTime;
            }
        }
        else{
            MoveToward(nextDestination);
        }

        if (attackTarget != null)
        {
            HandlePlayerDetection();
        }
        
        //자신의 구역이 아닌 경우.
        if(CheckIsSoFar()){
            MoveToward(nextDestination);    
        }

        //죽음
        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }

    }

    private bool CheckIsSoFar(){
        float distance = Vector3.Distance(transform.position, originalPosition);

        return distance > monsterData.movableArea;
    }
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    public float waitingTime = 2.0f;
    private void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        randomDirection += originalPosition;            //최초 위치를 기준으로 원을 그려서 이동 범위를 설정한다.
        nextDestination = randomDirection;
        nextDestination.y = 0;
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    private void HandlePlayerDetection()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);

        if (distanceToTarget <= monsterData.attackableRadius)
        {
            MonsterAttackHandler();
        }
        else
        {
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
            return true;
        }
        return false;
    }

    private void MoveToward(Vector3 destination)
    {
        animator.SetBool("Walk", true);
        Vector3 direction = (destination - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * monsterData.moveSpeed);
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * monsterData.moveSpeed);
        // if (!isAttackingTarget)
        // {
        //     animator.SetBool("Walk", true);
        //     Vector3 direction = (destination - transform.position).normalized;

        //     if (direction != Vector3.zero)
        //     {
        //         Quaternion targetRotation = Quaternion.LookRotation(direction);
        //         transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * monsterData.moveSpeed);
        //     }
        //     transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * monsterData.moveSpeed);
        // }
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

    GameObject loots;

    public void Down()
    {
        isDown = true;

        animator.SetTrigger("Down");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        Invoke("OnDownMonster", 1.5f);

        if(loots != null)
        {
            loots.SetActive(true);
            loots.GetComponent<MonsterLootHandler>().ShootLoots();
        }
    }
}