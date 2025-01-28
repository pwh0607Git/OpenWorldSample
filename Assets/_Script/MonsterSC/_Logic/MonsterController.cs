using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("MonsterData")]
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;             // UI는 동적 생성이 아닌 프리팹에 추가한 상태로 스폰하도록하기.
    public Animator animator;
    private CharacterController controller;

    [Header("MonsterState")]
    public int monsterCurHP;
    [SerializeField] float rotationSpeed = 20.0f;

    [Header("Flag")]
    private bool canTakeDamage = true;
    private bool isDown = false;
    private bool isLookingPlayerAction = false;
    private bool isIdle = true;                             //현재 플레이어와 전투중인지 아닌지 판별하는 flag

    private bool isAttackingTarget = false;

    [Header("Moving")]
    public Vector3 originalPosition;
    public Vector3 nextDestination;
    
    [Header("Monster-Attack")]
    [SerializeField] private Transform attackTarget;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        attackTarget = null;
        TryGetComponent(out animator);
        controller = GetComponentInChildren<CharacterController>();
        FixOriginalPosition();
        StartCoroutine(Coroutine_SetMonsterUI());
        loots = transform.GetComponentInChildren<MonsterLootHandler>().gameObject;
        SetNextDestination();
    }

    private void Update()
    {
        if(attackTarget == null){
            if(IsArrivingDestination(transform.position, nextDestination)){
                if(!isWaiting){
                    animator.SetBool("Walk", false);
                    isWaiting = true;
                    waitTimer = waitingTime;
                }
            }else{
                MoveToward(nextDestination);
            }
            //플레이어 미인식 상태.
            if (isWaiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    animator.SetBool("Walk", true);
                    isWaiting = false;
                    SetNextDestination();
                }
            }
        }else{
            // 추격 시작
            HandlePlayerDetection();
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }
    }

    public bool IsArrivingDestination(Vector3 position, Vector3 destination){
        return Vector3.Distance(NonYValue(position), NonYValue(destination)) <= 0.5f;
    }

    public void DownMonster(){
        Down();
    }

    void FixOriginalPosition(){
        var spawnController = GetComponentInChildren<ObjectSpawnInitController>();
        spawnController.SetOntheFloor();
        originalPosition = spawnController.originalPosition;
        transform.position = originalPosition;            
    }

    IEnumerator Coroutine_SetMonsterUI(){
        while(GetComponentInChildren<MonsterStateUIController>() == null){
            yield return null;
        }
        monsterUI = GetComponentInChildren<MonsterStateUIController>();
    }

    private bool CheckIsSoFar(){
        float distance = Vector3.Distance(NonYValue(transform.position), NonYValue(originalPosition));
        return distance > monsterData.movableArea;
    }

    private Vector3 NonYValue(Vector3 vec){
        Vector3 newVector = new Vector3(vec.x, 0,vec.z);
        return newVector;
    }

    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    public float waitingTime = 2.0f;

    public void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        nextDestination = NonYValue(randomDirection + originalPosition);
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }

    [SerializeField] float chasingSpeed = 10f;
    private void HandlePlayerDetection()
    {
        if(attackTarget == null) return;

        Vector3 directionToTarget = attackTarget.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;
        
        if (distanceToTarget <= monsterData.attackableRadius)
        {
            MonsterAttackHandler();
        }
        else
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        if (isAttackingTarget) return;
        MoveToward(attackTarget.position);
    }

    public void MoveToward(Vector3 destination)
    {
        if (isAttackingTarget || isWaiting) return;

        Vector3 moveDirection = NonYValue((destination - transform.position).normalized);
        float fixedSpeed = (attackTarget == null) ? monsterData.moveSpeed : monsterData.moveSpeed * chasingSpeed;
        
        if (controller.isGrounded)
        {
            animator.SetBool("Walk", true);
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);  
            }
        }
        else
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }

        controller.Move(moveDirection * fixedSpeed * Time.deltaTime);
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

        //공격 받으면 player 쳐다보기.
        // isIdle = false;
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
        if (!isDown && !isAttackingTarget && !isMonsterAttackCoolDown)
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
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, monsterData.attackDamageRadius);

        if(hitTargets.Length == 0){
            //공격 위치가 올바르지 못하다
            transform.rotation = Quaternion.Lerp(transform.rotation, attackTarget.transform.rotation, Time.deltaTime * 5.0f);
        }else{
            foreach(var target in  hitTargets)
            {
                if (target.CompareTag("Player"))
                {
                    target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(monsterData.attackPower);
                }
            }
        }
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    public void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Gizmos.DrawWireSphere(attackOffset, monsterData.attackDamageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterData.attackableRadius);
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
        isAttackingTarget = false;
    }

    private GameObject loots;

    public void Down()
    {
        isDown = true;
        animator.SetTrigger("Down");
        Invoke("OnDownMonster", 1.5f);
    }

    void OnDownMonster(){
        if(loots != null)
        {
            loots.SetActive(true);
            loots.GetComponent<MonsterLootHandler>().ShootLoots();
        }

        Destroy(gameObject);
    }
}