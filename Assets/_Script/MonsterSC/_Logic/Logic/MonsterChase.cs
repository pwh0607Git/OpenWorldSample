
using UnityEngine;

public class MonsterChase : MonoBehaviour
{
    private MonsterBlackBoard blackBoard;
    void Start(){
        blackBoard = GetComponentInChildren<MonsterBlackBoard>();
    }

    public void ChaseTarget()
    {
        if(blackBoard.isMonsterAttacking || blackBoard.isMonsterAttackCoolDown) return;
        if(Vector3.Distance(transform.position, blackBoard.player.transform.position) > blackBoard.monsterData.chasingRadius) return;
        blackBoard.MoveToward(blackBoard.player.position);   

    }

    public bool IsTargetInChasingRange(){
        if(blackBoard.player == null) return false;
        float distanceToTarget = Vector3.Distance(transform.position, blackBoard.player.position);
        if(distanceToTarget > blackBoard.monsterData.chasingRadius) return false;
        return true;
    }

    void OnDrawGizmos(){
        Vector3 dir = blackBoard.player.position - transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up, dir.normalized * blackBoard.monsterData.chasingRadius);
    }
}