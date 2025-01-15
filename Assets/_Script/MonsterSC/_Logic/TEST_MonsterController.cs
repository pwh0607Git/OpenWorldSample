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
    public int monsterCurHP;

    [Header("Flag")]
    private bool canTakeDamage = true;
    private bool isDown = false;

    private bool isAttackingTarget = false;
    private bool isDetectingTarget = false;

    [Header("Moveing")]
    private Vector3 originalPosition;

    private Vector3 nextDestination;
    
    [Header("Monster-Attack")]
    private Transform attackTarget;
    private void Start()
    {
        monsterCurHP = monsterData.HP;
        attackTarget = null;
        TryGetComponent(out animator);
        controller = GetComponentInChildren<CharacterController>();

        FixPosition();
        
        loots = transform.GetComponentInChildren<MonsterLootHandler>().gameObject;
        SetNextDestination();
    }

    private void Update()
    {
        //Chase Player
        if (attackTarget != null)
        {
            HandlePlayerDetection();
        }else{
            if(CheckIsSoFar() || attackTarget == null){
                MoveToward(nextDestination);    
            }

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

            if(isArrivingDestination(transform.position, nextDestination)){
                if(!isWaiting){
                    animator.SetBool("Walk", false);
                    isWaiting = true;
                    waitTimer = waitingTime;
                }
            }
            else{
                MoveToward(nextDestination);
            }
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }
    }

    bool isArrivingDestination(Vector3 position, Vector3 destination){
        position.y = 0;
        destination.y = 0;
        return Vector3.Distance(position, destination) <= 0.5f;
    }

    [Button]
    public void DownMonster(){
        Down();
    }

    void FixPosition()
    {
        controller.enabled = false;

        var spawnController = GetComponentInChildren<ObjectSpawnInitController>();
        spawnController.SetOntheFloor();
        originalPosition = spawnController.originalPosition;
        transform.position = originalPosition;

        controller.enabled = true;  
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

    public void SetAttackTarget(Transform target)           //<= MonsterDetection.sc
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

    private void ChasePlayer()
    {
        if (isAttackingTarget) return;

        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
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
        Debug.DrawRay(new Vector3(transform.position.x,transform.position.y+1,transform.position.z), direction.normalized,Color.red, monsterData.detectionRadius);
        if(Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    private void MoveToward(Vector3 destination)
    {
        if (isAttackingTarget || isWaiting) return;

        Vector3 moveDirection = (destination - transform.position).normalized;

        if (controller.isGrounded)
        {
            animator.SetBool("Walk", true);

            if (moveDirection != Vector3.zero)
            {
                float rotationSpeed = 5.0f; // 회전 속도
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);  
            }
        }
        else
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }

        controller.Move(moveDirection * monsterData.moveSpeed * Time.deltaTime);
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
        if (!isDown && isDetectingTarget && !isAttackingTarget && !isMonsterAttackCoolDown)
        {
            StartMonsterAttack(); 
        }
    }

    public void StartMonsterAttack(){
        Debug.Log($"{gameObject.name} 공격!");
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