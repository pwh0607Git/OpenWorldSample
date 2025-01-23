using UnityEngine;

public class MonsterControllerFromState : MonoBehaviour
{
    public IMonsterState currentState;  
    public Animator animator;
    public Transform attackTarget;
    public MonsterData MonsterData;

    public Vector3 nextDestination;

    [SerializeField] float rotationSpeed = 20.0f;
    
    [SerializeField] Vector3 originalPosition;
    [SerializeField] float movingAreaRedius;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        TransitionToState(MonsterStates.MonsterStateIdle.GetInstance());
    }

    private void Update()
    {
        currentState?.UpdateState(this);
    }

    public void TransitionToState(IMonsterState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
    }

    public void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * movingAreaRedius;
        nextDestination = NonYValue(randomDirection + originalPosition);
    }

    public bool IsArrivingDestination(Vector3 position, Vector3 destination){
        return Vector3.Distance(NonYValue(position), NonYValue(destination)) <= 0.5f;
    }

    private Vector3 NonYValue(Vector3 vec){
        Vector3 newVector = new Vector3(vec.x, 0,vec.z);
        return newVector;
    }

    public void MoveToward(Vector3 destination)
    {
        Vector3 moveDirection = (destination - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
