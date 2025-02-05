using UnityEngine;

public class MonsterDetection : MonoBehaviour
{
    private MonsterBlackBoard blackBoard;
    [SerializeField] private float detectionAngle = 80f;
    void Start(){
        blackBoard = GetComponentInChildren<MonsterBlackBoard>();
    }
    
    public bool IsTargetInDetectionRange()
    {
        if (blackBoard.player == null) return false;
        Vector3 directionToTarget = blackBoard.player.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget > blackBoard.monsterData.detectionRadius)
            return false;

        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget.normalized);
        if (angleToTarget > detectionAngle / 2)
            return false;
        return true;
    }
    public bool IsExistingObject()
    {
        Vector3 directionToTarget = (blackBoard.player.position - transform.position).normalized;
        if(Physics.Raycast(transform.position, directionToTarget, out RaycastHit hit, blackBoard.monsterData.detectionRadius, LayerMask.GetMask("Level"))){
            return true;
        }
        return false;
    }

    public bool IsTargetInChasingRange(){
        if(blackBoard.player == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, blackBoard.player.position);

        // after : detectionRadius -> chasingRadius 변경 요망
        if(distanceToTarget > blackBoard.monsterData.detectionRadius) return false;
        return true;
    }
    
    public void ChaseTarget()
    {
        if(blackBoard.isMonsterAttacking || blackBoard.isMonsterAttackCoolDown) return;
        blackBoard.MoveToward(blackBoard.player.position);
    }
}
