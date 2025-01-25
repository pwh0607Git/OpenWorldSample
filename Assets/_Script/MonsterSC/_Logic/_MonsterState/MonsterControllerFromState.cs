using UnityEditorInternal;
using UnityEngine;

public class MonsterControllerFromState : MonoBehaviour
{
    [Header("MonsterData")]
    public IMonsterState currentState;
    public MonsterData monsterData;
    public Animator animator;
    public Transform attackTarget;


    [SerializeField] Vector3 originalPosition;
    public Vector3 nextDestination;

    [SerializeField] float rotationSpeed = 20.0f;
    [SerializeField] float movingAreaRedius;
    [SerializeField] float moveSpeed;
    [SerializeField] float chasingSpeed= 5f;

    MonsterDetectionObserver detection;
    bool isAttackingTarget = false;

    private CharacterController controller;
    private void Start()
    {
        InitMonster();
        TransitionToState(MonsterStates.MonsterStateIdle.GetInstance());
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    void InitMonster(){
        TryGetComponent(out animator);
        TryGetComponent(out controller);
        detection = GetComponentInChildren<MonsterDetectionObserver>();
        detection.OnTargetDetect += HandleTargetDetecte;
        detection.OnTargetLost += HandleTargetLost;
    }

    void HandleTargetDetecte(Transform target)
    {
        attackTarget = target;
        TransitionToState(MonsterStates.MonsterStateChase.GetInstance());
    }

    void HandleTargetLost()
    {
        attackTarget = null;
        TransitionToState(MonsterStates.MonsterStateIdle.GetInstance());
    }

    public void TransitionToState(IMonsterState newState)
    {
        Debug.Log($"상태 변환 : {currentState} -> {newState}");
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    //MonsterController
    public void MoveToward(Vector3 destination)
    {
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

    // MonsterState-Idle..
    public void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movingAreaRedius;
        nextDestination = NonYValue(randomDirection + originalPosition);
    }

    public bool IsArrivingDestination(Vector3 position, Vector3 destination)
    {
        return Vector3.Distance(NonYValue(position), NonYValue(destination)) <= 0.1f;
    }

    private Vector3 NonYValue(Vector3 vec)
    {
        Vector3 newVector = new Vector3(vec.x, 0, vec.z);
        return newVector;
    }

    // MonsterState-Chase
    public void ChasePlayer()
    {
        if (isAttackingTarget) return;
        MoveToward(attackTarget.position);
    }

    public void SetAttackTarget(Transform target){
        attackTarget = target;
    }
}
