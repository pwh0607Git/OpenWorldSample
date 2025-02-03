using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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
        Debug.Log($"🔥 몬스터가 {damage}의 피해를 받음! 현재 HP: {monsterCurHP}");
        
        // if (monsterCurHP <= 0)
        // {
        //     // Die(); // 몬스터 사망 처리
        //     Debug.Log("몬스터 사망...");
        //     return;
        // }
        
        isDamaged = true;
        StartCoroutine(Coroutine_ResetDamageState());
    }

    private NodeState HandleDamageAnim()
    {
        if (isDamaged && !animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))              // 🔥 `isDamaged`가 true이면 애니메이션 실행하도록 수정
        {
            animator.SetTrigger("Damaged");
            return NodeState.Success;
        }   
        Debug.Log("실패...");
        return NodeState.Failure; // 실행되지 않을 경우 Failure 반환
    }   

    private NodeState IsDamageAnimDone()
    {  
        AnimatorStateInfo animState = animator.GetCurrentAnimatorStateInfo(0);
        if (isDamaged)
        {
            return NodeState.Running;
        }

        return NodeState.Success;
    }

    IEnumerator Coroutine_ResetDamageState()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("피격상태 종료...");
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

        return true;
    }

    private bool IsTargetInChasingRange(){
        if(player == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, player.position);

        // after : detectionRadius -> chasingRadius 변경 요망
        if(distanceToTarget > monsterData.detectionRadius) return false;
        return true;
    }
    #endregion

    private bool CheckTargetInAttackRange()
    {
        if (player == null) return false;
        if (isMonsterAttackCoolDown || isAttacking) return false;

        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        return distanceToTarget <= monsterData.attackableRadius;
    }

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

    public void ChaseTarget()
    {
        if(isAttacking || isMonsterAttackCoolDown) return;
        MoveToward(player.position);
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        animator.SetBool("Walk", false); // Idle 상태 유지
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

    #region 공용 파트

    private void Start()
    {
        InitMonsterData();
        SetNextDestination();
        player = GameObject.FindWithTag("Player").transform;

        rootNode = new Selector(new List<BTNode>
        {
            // new Sequence(new List<BTNode>{
            //     new ConditionNode(IsDownMonster),
            //     new ActionNode(DownMonster)
            // }),
            new Sequence(new List<BTNode>
            {
                new ConditionNode(CheckTakeDamage),      // 몬스터가 피해를 입었는가?
                new ActionNode(HandleDamageAnim),
                new ActionNode(IsDamageAnimDone),        // 피격 애니메이션 종료 대기
                new LookAtTargetNode(transform, player, animator, rotationSpeed),  // 플레이어 바라보기
                // new ActionNode(ChaseTarget)                 // 플레이어 추격
            }),
            // new Sequence(new List<BTNode>
            // {
            //     new ConditionNode(CheckTargetInAttackRange),
            //     new ActionNode(AttackTarget)
            // }),
            // new Sequence(new List<BTNode>
            // {
            //     new ConditionNode(IsTargetInDetectionRange),
            //     new ActionNode(ChaseTarget)
            // }),
            // new ActionNode(Patrol)    
        });
    }

    private void Update()
    {
        rootNode.Evaluate();
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
    private float delayTime = 3.0f;
    private float delayTimer = 0f;

    private void ActionDelay(/*float t*/){
        if (delayTimer < delayTime)
        {
            delayTimer += Time.deltaTime;
            Debug.Log($"Delay... {delayTimer}");        
        }
    }
    #endregion
}