using UnityEngine;

public class MonsterDetectionHandler : MonoBehaviour
{
    MonsterController monsterController;
    
    [SerializeField] GameObject inDectionAreaObject;
    [SerializeField] GameObject inAttackAreaObject;
    
    private SphereCollider detectionCollider;

    [SerializeField] float detectionRadius = 10f;   
    [SerializeField] float detectionAngle = 70f;

    private void Start()
    {
        monsterController = transform.GetComponentInParent<MonsterController>();
        
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;
    }

        private void Update()
    {
        if (inDectionAreaObject != null) // 감지된 객체가 있을 때
        {
            Vector3 directionToPlayer = inDectionAreaObject.transform.position - transform.position;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

            if(IsExistingObject(directionToPlayer)) return;

            // 부채꼴 범위 안으로 들어온 경우
            if (angleToPlayer < detectionAngle / 2)
            {
                if (inAttackAreaObject == null) // 처음으로 부채꼴 범위 안으로 들어온 경우
                {
                    inAttackAreaObject = inDectionAreaObject;
                    monsterController.SetAttackTarget(inAttackAreaObject.transform); // 타겟 설정
                    Debug.Log("플레이어가 부채꼴 범위 안으로 들어옴!");
                }
            }
            else
            {
                if (inAttackAreaObject != null) // 부채꼴 범위 밖으로 나간 경우
                {
                    inAttackAreaObject = null;
                    Debug.Log("플레이어가 부채꼴 범위 밖으로 나감.");
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Angle 안에 들어왔을 때!
        if (other.CompareTag("Player"))
        {
            inDectionAreaObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inDectionAreaObject = null;
            inAttackAreaObject = null; // 부채꼴 범위 객체도 초기화
            monsterController.SetAttackTarget(null);
        }
    }

    bool IsExistingObject(Vector3 direction)
    {
        if(Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    private bool IsPlayerInDetectionArea(GameObject target)
    {
        if (target == null) return false;
        Vector3 directionToPlayer = target.transform.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > detectionRadius)
        {
            return false;
        }

        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer.normalized);

        if (angleToPlayer > detectionAngle / 2)
        {
            return false;
        }

        return true;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Vector3 forward = transform.forward * detectionRadius;
        Quaternion leftRayRotation = Quaternion.Euler(0, -detectionAngle / 2, 0);
        Quaternion rightRayRotation = Quaternion.Euler(0, detectionAngle / 2, 0);

        Vector3 leftRay = leftRayRotation * forward;
        Vector3 rightRay = rightRayRotation * forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }
}
