using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterAI : MonoBehaviour
{
    private NavMeshAgent agent;

    public Transform[] patrolPoints;    // 몬스터가 이동할 특정 위치들
    public Transform initPoint;         // 몬스터의 초기 위치.
    
    public float waitTime = 2f;         // 각 지점에서 대기 시간, 차후 랜덤한 수열의 형태로 구현... 2초 3초 10초 등등...
    public float moveRadius = 10f;      // 랜덤 이동 반경
    public float speed = 10f;

    private int currentPointIndex;
    private bool waiting;
    private float waitTimer;

    private MonsterController monsterController;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        monsterController = GetComponent<MonsterController>();

        if (patrolPoints.Length > 0)
        {
            MoveToNextPatrolPoint();
        }
        else
        {
            MoveToRandomPoint();
        }
    }

    private void Update()
    {
        DetectPlayer();

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!waiting)
            {
                waiting = true;
                waitTimer = waitTime;
                monsterController.ChangeState(MonsterAnimState.Idle);
            }
        }

        if (waiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                monsterController.ChangeState(MonsterAnimState.Walk);
                waiting = false;
                if (patrolPoints.Length > 0)
                {
                    MoveToNextPatrolPoint();
                }
                else
                {
                    MoveToRandomPoint();
                }
            }
        }
    }

    public Transform player;
    public float detectionRadius = 10f;     // 감지 거리
    public float detectionAngle = 80f;      // 감지 각도 (부채꼴 범위)

    private void DetectPlayer()
    {
        if (IsPlayerInDetectionArea())
        {
            ChasePlayer();
        }
    }

    private void ChasePlayer()
    {
        transform.LookAt(player);
        Vector3 dir = player.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }

    private bool IsPlayerInDetectionArea()
    {
        if (player == null) return false;
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRadius)
        {
            return false;       // 감지 반경 밖
        }

        // 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > detectionAngle / 2)
        {
            return false;       // 감지 각도 밖
        }

        return true;            //모든 조건을 만족한 경우...
    }
    
    private void BackInitPoint()
    {
        transform.LookAt(initPoint);
        Vector3 dir = initPoint.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, initPoint.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }

    private void MoveToNextPatrolPoint()
    {
        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    private void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -detectionAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, detectionAngle / 2, 0);

        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }
}