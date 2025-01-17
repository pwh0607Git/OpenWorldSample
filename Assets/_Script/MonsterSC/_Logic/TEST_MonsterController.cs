using System.Collections;
using UnityEngine;
using NaughtyAttributes;

public class TEST_MonsterController : MonoBehaviour
{
    [Header("MonsterData")]
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;
    private Animator animator;
    private CharacterController controller;

    [Header("MonsterState")]
    public int monsterCurHP;

    [Header("Flag")]
    private bool canTakeDamage = true;
    private bool isDown = false;

    private bool isAttackingTarget = false;

    [Header("Moveing")]
    private Vector3 originalPosition;

    private Vector3 nextDestination;
    
    [Header("Monster-Attack")]
    [SerializeField] private Transform attackTarget;

    [ReadOnly, SerializeField] float print_distance = 0f;
    private void Start()
    {
        monsterCurHP = monsterData.HP;
        attackTarget = null;
        TryGetComponent(out animator);
        controller = GetComponentInChildren<CharacterController>();

        FixOriginalPosition();
        
        loots = transform.GetComponentInChildren<MonsterLootHandler>().gameObject;
        SetNextDestination();
    }

    private void Update()
    {
        if(attackTarget == null){
            if(isArrivingDestination(transform.position, nextDestination)){
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

    bool isArrivingDestination(Vector3 position, Vector3 destination){
        return Vector3.Distance(NonYValue(position), NonYValue(destination)) <= 0.5f;
    }

    [Button]
    public void DownMonster(){
        Down();
    }

    void FixOriginalPosition()
    {
        controller.enabled = false;

        var spawnController = GetComponentInChildren<ObjectSpawnInitController>();
        spawnController.SetOntheFloor();
        originalPosition = spawnController.originalPosition;
        transform.position = originalPosition;

        controller.enabled = true;  
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
    private void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        nextDestination = NonYValue(randomDirection + originalPosition);
    }

    public void SetAttackTarget(Transform target)           //<= MonsterDetection.sc
    {
        attackTarget = target;
    }
    [SerializeField] float chasingSpeed = 2.5f;
    
    private void HandlePlayerDetection()
    {
        if(attackTarget == null || IsExistingObject(attackTarget.transform.position)) return;

        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);
        print_distance = distanceToTarget;
        
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

    bool IsExistingObject(Vector3 direction)
    {
        if(Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    private void MoveToward(Vector3 destination)
    {
        if (isAttackingTarget || isWaiting) return;

        Vector3 moveDirection = NonYValue((destination - transform.position).normalized);
        float fixedSpeed = (attackTarget == null) ? monsterData.moveSpeed : monsterData.moveSpeed * chasingSpeed;
        
        if (controller.isGrounded)
        {
            animator.SetBool("Walk", true);
            if (moveDirection != Vector3.zero)
            {
                float rotationSpeed = 20.0f;
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
        Debug.Log("MonsterAttackHandler");
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
        Debug.Log("Perform Attack");
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, monsterData.attackDamageRadius);

        if(hitTargets.Length == 0){
            //공격 위치가 올바르지 못하다
            Debug.Log("위치가 올바르지 못함... 바라보기");
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

    GameObject loots;

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