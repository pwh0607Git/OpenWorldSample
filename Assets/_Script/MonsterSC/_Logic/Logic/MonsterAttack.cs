using System.Collections;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private MonsterBlackBoard blackBoard;
    
    private bool isMonsterAttackCoolDown = false;
    
    void Start(){
        blackBoard = GetComponentInChildren<MonsterBlackBoard>();
    }

    public void AttackTarget()
    {
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, blackBoard.monsterData.attackDamageRadius);

        if (hitTargets.Length == 0)
        {
           transform.rotation = Quaternion.Lerp(transform.rotation, blackBoard.player.transform.rotation, Time.deltaTime * 5.0f);
        }
        else
        {
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                {
                    blackBoard.isMonsterAttacking = true;
                    blackBoard.animator.SetBool("Walk", false);
                    blackBoard.animator.SetTrigger("Attack");
                }
            }
        }
    }

    public bool CheckTargetInAttackRange()
    {
        if (blackBoard.player == null) return false;
        if (isMonsterAttackCoolDown || blackBoard.isMonsterAttacking) return false;

        float distanceToTarget = Vector3.Distance(transform.position, blackBoard.player.position);
        return distanceToTarget <= blackBoard.monsterData.attackableRadius;
    }

    //animation Event
    public void PerformAttack(){
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, blackBoard.monsterData.attackDamageRadius);

        if(hitTargets.Length == 0){
            transform.rotation = Quaternion.Slerp(transform.rotation, blackBoard.player.transform.rotation, Time.deltaTime * 5.0f);
        }else{
            foreach(var target in  hitTargets)
            {
                if (target.CompareTag("blackBoard.player"))
                {
                    target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(blackBoard.monsterData.attackPower);
                }
            }
        }
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        blackBoard.animator.SetBool("Walk", false);
        yield return new WaitForSeconds(blackBoard.monsterData.attackCooldown);
        isMonsterAttackCoolDown = false;
        blackBoard.isMonsterAttacking = false;
    }
}