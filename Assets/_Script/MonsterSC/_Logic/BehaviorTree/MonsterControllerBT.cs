using System;
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

    private void Start()
    {
        InitMonsterData();
        SetNextDestination();
        player = GameObject.FindWithTag("Player").transform;

        rootNode = new Selector(new List<BTNode>
        {
            new Sequence(new List<BTNode>
            {
                new ConditionNode(CheckTakeDamage),
                new ActionNode(HandleDamage)
            }),
            new Sequence(new List<BTNode>
            {
                //공격
                new ConditionNode(CheckTargetInAttackRange),
                new ActionNode(AttackTarget)
            }),
            new Sequence(new List<BTNode>
            {   
                // 추적
                new ConditionNode(IsTargetInDetectionRange),
                new ActionNode(ChaseTarget)
            }),
            new Sequence(new List<BTNode>
            {
                // 배회
                new ActionNode(Patrol),
                new ConditionNode(IsTargetInDetectionRange),
            }),
        });

        #region Test
        #endregion
    }

    #region TakeDamage
    private bool isDamaged = false;
    private float damageCooldown = 0.5f;

    private bool CheckTakeDamage()
    {
        return isDamaged;
    }

    //피격 로직 수행!
    private void HandleDamage()
    {
        StartCoroutine(Coroutine_ResetDamageState());
    }

    IEnumerator Coroutine_ResetDamageState()
    {
        yield return new WaitForSeconds(damageCooldown);
        isDamaged = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDamaged) return;

        isDamaged = true;
        Debug.Log($"Take Damage!");
        monsterCurHP -= damage;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            // 공격 애니메이션을 수행중이지 않으면 데미지 누적만하기
            animator.SetTrigger("Damaged");
        }
    }
    #endregion

    void InitMonsterData()
    {
        TryGetComponent(out animator);
        TryGetComponent(out controller);
        monsterCurHP = monsterData.HP;
    }

    private void Update()
    {
        rootNode.Evaluate();

        #region Test
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(10);
        #endregion
    }

    private bool IsTargetInDetectionRange()
    {
        #region Test
        if (player == null) return false;
        #endregion

        Vector3 directionToTarget = player.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > monsterData.detectionRadius)
            return false;

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (angleToTarget > detectionAngle / 2)
            return false;

        return true;
    }

    private bool isAttacking = false;

    private bool CheckTargetInAttackRange()
    {
        if (player == null) return false;
        if (isMonsterAttackCoolDown) return false;

        float distanceToTarget = Vector3.Distance(transform.position, player.position);
        return distanceToTarget <= monsterData.attackableRadius;
    }

    void AttackTarget()
    {
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, monsterData.attackDamageRadius);

        if (hitTargets.Length == 0)
        {
            //공격 위치가 올바르지 못하다
            transform.rotation = Quaternion.Lerp(transform.rotation, player.transform.rotation, Time.deltaTime * 5.0f);
        }
        else
        {
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                {
                    target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(monsterData.attackPower);
                }
            }
        }

        StartCoroutine(Coroutine_AttackCoolDown());
    }
    #region Attack
    bool isMonsterAttackCoolDown = false;
    float monsterAttackCooldownTime = 2.0f;

    void ChaseTarget()
    {
        MoveToward(player.position);
    }
    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        yield return new WaitForSeconds(monsterAttackCooldownTime);
        isMonsterAttackCoolDown = false;
    }
    #endregion

    #region Patrol
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    public float waitingTime = 2.0f;

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
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * monsterData.movingAreaRedius;
        nextDestination = (randomDirection + originalPosition).FlattenY();
    }

    public bool CheckArrivingDestination(Vector3 position, Vector3 destination)
    {
        return Vector3.Distance(position.FlattenY(), destination.FlattenY()) <= 0.1f;
    }
    #endregion

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

    #region monsterDestroy
    public static event Action<GameObject> OnMonsterDestroyed; // 이벤트 선언

    private void OnDestroy()
    {
        OnMonsterDestroyed?.Invoke(gameObject); // 몬스터가 삭제될 때 이벤트 발생
    }
    #endregion
}