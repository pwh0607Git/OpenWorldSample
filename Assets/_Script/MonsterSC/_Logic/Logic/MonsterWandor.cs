using UnityEngine;

public class MonsterWandor : MonoBehaviour
{
    MonsterBlackBoard blackBoard;
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    [SerializeField] float waitingTime = 2.0f;

    void Start(){  
        blackBoard = GetComponentInChildren<MonsterBlackBoard>();
        SetNextDestination();
    }
    public void Wandor()
    {
        Debug.Log("Wandoring...");
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                blackBoard.animator.SetBool("Walk", true);
                isWaiting = false;
                SetNextDestination();
            }
        }

        if (CheckArrivingDestination(transform.position, blackBoard.nextDestination))
        {
            if (!isWaiting)
            {
                blackBoard.animator.SetBool("Walk", false);
                isWaiting = true;
                waitTimer = waitingTime;
            }
        }
        else
        {
           blackBoard.MoveToward(blackBoard.nextDestination);
        }
    }

    private void SetNextDestination()
    {
        Vector3 randomDirection = Vector3.zero;
        do{
            randomDirection = Random.insideUnitSphere * blackBoard.monsterData.movingAreaRedius;
        }while(Vector3.Distance(randomDirection, transform.position) < 3.0f);
        blackBoard.nextDestination = (randomDirection + blackBoard.originalPosition).FlattenY();
    }

    public bool CheckArrivingDestination(Vector3 position, Vector3 destination)
    {
        return Vector3.Distance(position.FlattenY(), destination.FlattenY()) <= 0.1f;
    }
}
