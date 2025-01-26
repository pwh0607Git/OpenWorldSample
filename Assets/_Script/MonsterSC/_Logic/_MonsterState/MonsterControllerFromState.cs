using MonsterStates;
using UnityEngine;

public class MonsterControllerFromState : MonoBehaviour
{
    [Header("MonsterData")]
    public IMonsterState currentState;
    public MonsterData monsterData;
    public Animator animator;
    [SerializeField] Transform attackTarget;
    private CharacterController controller;

    public Vector3 originalPosition;

    [SerializeField] float rotationSpeed = 20.0f;
    [SerializeField] float movingAreaRedius;
    [SerializeField] float moveSpeed;
    [SerializeField] float chasingSpeed;

    [SerializeField] Vector3 nextDestination;
    [SerializeField] MonsterDetectionObserver detection;
    bool isAttackingTarget = false;

    private void Start()
    {
        InitMonster();
        TransitionToState(new MonsterStateIdle());
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    void InitMonster(){
        TryGetComponent(out animator);
        TryGetComponent(out controller);
        detection = GetComponentInChildren<MonsterDetectionObserver>();

        detection.OnTargetDetected += HandleTargetDetect;
        detection.OnTargetLost += HandleTargetLost;
    }

    void HandleTargetDetect(Transform target)
    {
        Debug.Log("Target Detect!");
        attackTarget = target;
        TransitionToState(new MonsterStateChase());
    }

    void HandleTargetLost()
    {
        Debug.Log("Target Lost!");
        attackTarget = null;
        TransitionToState(new MonsterStateIdle());
    }

    public void TransitionToState(IMonsterState newState)
    {
        if (currentState == newState)
        {
            return;                     // 동일한 상태로의 전환 방지
        }

        Debug.Log($"상태 변환 : {currentState} -> {newState}");
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void MoveToward(Vector3 destination)
    {
        Vector3 moveDirection = ((destination - transform.position).normalized).FlattenY();
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

    public void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movingAreaRedius;
        nextDestination = (randomDirection + originalPosition).FlattenY();
    }

    public bool IsArrivingDestination(Vector3 position, Vector3 destination)
    {
        return Vector3.Distance(position.FlattenY(), destination.FlattenY()) <= 0.1f;
    }

    // MonsterState-Chase
    public void ChasePlayer()
    {
        if (isAttackingTarget) return;
        MoveToward(attackTarget.position);
    }

    public Transform GetAttackTarget()
    {
        return attackTarget;
    }

    public void SetAttackTarget(Transform target)
    {
        attackTarget = target;
    }
}
