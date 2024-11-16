using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterAI : MonoBehaviour
{
    public Transform[] patrolPoints;    // 몬스터가 이동할 특정 위치들
    public Transform initPoint;
    
    public float waitTime = 2f;         // 각 지점에서 대기 시간
    public float moveRadius = 10f;      // 랜덤 이동 반경
    public float speed = 10f;

    private NavMeshAgent agent;
    private int currentPointIndex;
    private bool waiting;
    private float waitTimer;
    private bool returning;             //최초 위치로 복귀하는 중인 상태.

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

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
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!waiting)
            {
                waiting = true;
                waitTimer = waitTime;
            }
        }

        if (waiting)
        {
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
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
        DetectPlayer();
    }

    public Transform player;
    public float detectionRadius = 10f;     // 감지 거리
    public float detectionAngle = 80f;      // 감지 각도 (부채꼴 범위)
    
    public float backDistance = 50f;

    private void DetectPlayer()
    {
        if (IsPlayerInDetectionArea())
        {
            ChasePlayer();
        }
        else
        {
            //BackInitPoint();
        }
    }

    private void ChasePlayer()
    {
        //너무 멀리 넘어왔다면...
        if (Vector3.Distance(transform.position, initPoint.position) < backDistance)
        {
            transform.LookAt(player);
            Vector3 dir = player.position - transform.position;
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
        }
        else
        {
            BackInitPoint();
            returning = true;
            Debug.Log("너무 멀리나와서 최초 위치로 이동합니다...");
        }
    }

    private bool IsPlayerInDetectionArea()
    {
        if (player == null) return false;

        // 거리 계산
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRadius)
        {
            return false; // 감지 반경 밖
        }

        // 각도 계산
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > detectionAngle / 2)
        {
            return false; // 감지 각도 밖
        }

        return true; // 거리와 각도가 모두 조건을 만족
    }

    private void OnDrawGizmos()
    {
        // 디버그용 부채꼴 시각화
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

    private void BackInitPoint()
    {
        transform.LookAt(initPoint);
        Vector3 dir = initPoint.position - transform.position;
        transform.position = Vector3.MoveTowards(transform.position, initPoint.position, speed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * speed);
    }

    private void MoveToNextPatrolPoint()
    {
        // 순차적으로 다음 목표 위치 설정
        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    private void MoveToRandomPoint()
    {
        // 랜덤한 위치를 NavMesh 위에 생성
        Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
        randomDirection += transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, moveRadius, NavMesh.AllAreas))
        {
            agent.destination = hit.position;
        }
    }
}