using System;
using UnityEngine;

public class MonsterDetectionObserver : MonoBehaviour
{
    public event Action<Transform> OnTargetDetected;
    public event Action OnTargetLost;

    [SerializeField] Transform target;

    [SerializeField] float detectionRadius = 10f;
    [SerializeField] float detectionAngle = 70f;

    bool isTargetDetected;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        isTargetDetected = false;
    }

    private void Update()
    {
        DetectTarget();
    }

    void DetectTarget()
    {
        if (target == null) return;

        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (IsTargetInDetectionArea(directionToTarget.normalized, distanceToTarget) && !IsExistingObject(directionToTarget.normalized))
        {
            if (!isTargetDetected)
            {
                isTargetDetected = true;
                OnTargetDetected?.Invoke(target);
            }
        }
        else
        {
            if (isTargetDetected)
            {
                isTargetDetected = false;
                OnTargetLost?.Invoke();
            }
        }
    }

    bool IsExistingObject(Vector3 directionToTarget)
    {
        if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    private bool IsTargetInDetectionArea(Vector3 directionToTarget, float distanceToTarget)
    {
        if (target == null) return false;
        if (distanceToTarget > detectionRadius) return false;

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget > detectionAngle / 2) return false;
        
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
