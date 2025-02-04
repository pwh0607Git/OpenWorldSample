using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterControllerBT : MonoBehaviour
{
    private BTNode rootNode;
    [SerializeField] private Transform player;
    private CharacterController controller;
    private Animator animator;
    private MonsterStateUIController monsterUI;

    [Header("Monster-State")]
    private int monsterCurHP = 0;

    [Header("Monster-Properties")]
    [SerializeField] private MonsterData monsterData;
    [SerializeField] private float detectionAngle = 80f;
    private float rotationSpeed = 20.0f;
    private float speedWeight = 3.0f;

    [Header("Logic-Properties")]
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 nextDestination;

    #region MonsterUI
    public event System.Action<int> OnMonsterDamaged;

    #endregion

    #region TakeDamage
    [SerializeField] bool isDamaged;
    [SerializeField] float noDamageCooldown = 0.5f;
    
    private bool CheckTakeDamage()
    {
        return isDamaged;
    }

    public void TakeDamage(int damage)
    {
        if (isDamaged) return;      //중복 피격 방지

        monsterCurHP -= damage;
        
        // if (monsterCurHP <= 0)
        // {
        //     // Die(); // 몬스터 사망 처리
        //     Debug.Log("몬스터 사망...");
        //     return;
        // }
        
        isDamaged = true;
        OnMonsterDamaged?.Invoke(monsterCurHP);
        StartCoroutine(Coroutine_ResetDamageState());
    }

    private void HandleDamageAnim()
    {
        if (isDamaged && !animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))              // 🔥 `isDamaged`가 true이면 애니메이션 실행하도록 수정
        {
            animator.SetTrigger("Damaged");
        }   
    }

    IEnumerator Coroutine_ResetDamageState()
    {  
        // 데미지 화력 조절
        yield return new WaitForSeconds(noDamageCooldown);
        isDamaged = false;
    }
    #endregion

    void InitMonsterData()
    {
        TryGetComponent(out animator);
        TryGetComponent(out controller);
        monsterCurHP = monsterData.HP;
        isDamaged = false;
    }

    #region Chase
    private bool IsTargetInDetectionRange()
    {
        if (player == null) return false;
        Vector3 directionToTarget = player.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > monsterData.detectionRadius)
            return false;

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (angleToTarget > detectionAngle / 2)
            return false;

        Debug.Log("player 추격 준비!");
        return true;
    }
    bool IsExistingObject()
    {
        Vector3 directionToTarget = (player.position - transform.position).normalized;
        if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    private bool IsTargetInChasingRange(){
        if(player == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, player.position);

        // after : detectionRadius -> chasingRadius 변경 요망
        if(distanceToTarget > monsterData.detectionRadius) return false;
        return true;
    }
    
    public void ChaseTarget()
    {
        if(isAttacking || isMonsterAttackCoolDown) return;
        Debug.Log("Player 추격중...");
        MoveToward(player.position);
    }
    #endregion

    [SerializeField] bool isAttacking = false;
    [SerializeField] bool isMonsterAttackCoolDown = false;
    float monsterAttackCooldownTime = 2.0f;

    #region Attack
    void AttackTarget()
    {
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, monsterData.attackDamageRadius);

        if (hitTargets.Length == 0)
        {
           transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 5.0f);
        }
        else
        {
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                {
                    isAttacking = true;
                    animator.SetBool("Walk", false);
                    animator.SetTrigger("Attack");
                }
            }
        }
    }

    private bool CheckTargetInAttackRange()
    {
        if (player == null) return false;
        if (isMonsterAttackCoolDown || isAttacking) return false;

        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        return distanceToTarget <= monsterData.attackableRadius;
    }

    //animation Event
    public void PerformAttack(){
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, monsterData.attackDamageRadius);

        if(hitTargets.Length == 0){
            //공격 위치가 올바르지 못하다
            transform.rotation = Quaternion.Slerp(transform.rotation, player.transform.rotation, Time.deltaTime * 5.0f);
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

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        animator.SetBool("Walk", false);
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
        isAttacking = false;
    }
    #endregion

    #region Patrol
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    [SerializeField] float waitingTime = 2.0f;

    void Patrol()
    {
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

        if (CheckArrivingDestination(transform.position, nextDestination))
        {
            if (!isWaiting)
            {
                animator.SetBool("Walk", false);
                isWaiting = true;
                waitTimer = waitingTime;
            }
        }
        else
        {
            MoveToward(nextDestination);
        }
    }

    public void SetNextDestination()
    {
        Vector3 randomDirection = Vector3.zero;
        do{
            randomDirection = Random.insideUnitSphere * monsterData.movingAreaRedius;
        }while(Vector3.Distance(randomDirection, transform.position) < 3.0f);
        nextDestination = (randomDirection + originalPosition).FlattenY();
    }

    public bool CheckArrivingDestination(Vector3 position, Vector3 destination)
    {
        return Vector3.Distance(position.FlattenY(), destination.FlattenY()) <= 0.1f;
    }
    #endregion

    #region Down
    private bool isDown;
    
    public bool IsDownMonster(){
        return isDown && monsterCurHP <= 0;
    }

    public void DownMonster(){
        Debug.Log("monster Down...!");
        animator.SetTrigger("Down");
        Invoke("DestroyMonster", 1f);
    }

    private void DestroyMonster(){
        //Loot Handler Code Part
        Destroy(gameObject);
    }
    #endregion

    #region UI Part

    #endregion

    #region Public Part

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        InitMonsterData();
        SetNextDestination();
        SetBTNode();
    }

    void SetBTNode(){
        rootNode = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>{
                new ConditionNode(IsDownMonster),
                new ActionNode(DownMonster)
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(CheckTakeDamage),      
                new ActionNode(HandleDamageAnim),
                new WaitNode(1f),
                new LookAtTargetNode(transform, player, animator, rotationSpeed),  
                new ActionNode(ChaseTarget)                 
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(CheckTargetInAttackRange),
                new ActionNode(AttackTarget)
            }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(IsTargetInDetectionRange),
                new ConditionNode(IsExistingObject),
                new ActionNode(ChaseTarget)
            }),
            new ActionNode(Patrol)    
        });
    }

    private void Update()
    {
        rootNode.Evaluate();
        #region  Test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
        #endregion
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, monsterData.detectionRadius);

        Vector3 forward = transform.forward * monsterData.detectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -detectionAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, detectionAngle / 2, 0);

        //Detection Gizmo
        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);

        //Attack Gizmo
        Gizmos.color = Color.red;
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Gizmos.DrawWireSphere(attackOffset, monsterData.attackDamageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, monsterData.attackableRadius);
    }

    public void MoveToward(Vector3 destination)
    {
        Vector3 moveDirection = ((destination - transform.position).normalized).FlattenY();
        float fixedSpeed = (player == null) ? monsterData.moveSpeed : monsterData.moveSpeed * speedWeight;

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
    #endregion
}