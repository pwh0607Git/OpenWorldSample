using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    private MonsterBlackBoard blackboard;

    private bool isMonsterAttackCoolDown = false;
    [SerializeField] private float detectionAngle = 80f;
     void AttackTarget()
    {
        Vector3 attackOffset = transform.localPosition + Vector3.up / 2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, blackboard.monsterData.attackDamageRadius);

        if (hitTargets.Length == 0)
        {
           transform.rotation = Quaternion.Lerp(transform.rotation, blackboard.player.transform.rotation, Time.deltaTime * 5.0f);
        }
        else
        {
            foreach (var target in hitTargets)
            {
                if (target.CompareTag("Player"))
                {
                    blackboard.isAttacking = true;
                    blackboard.animator.SetBool("Walk", false);
                    blackboard.animator.SetTrigger("Attack");
                }
            }
        }
    }

    private bool CheckTargetInAttackRange()
    {
        if (blackboard.player == null) return false;
        if (isMonsterAttackCoolDown || blackboard.isAttacking) return false;

        float distanceToTarget = Vector3.Distance(transform.position, blackboard.player.position);
        return distanceToTarget <= blackboard.monsterData.attackableRadius;
    }

    //animation Event
    public void PerformAttack(){
        Vector3 attackOffset = transform.localPosition + Vector3.up/2 + transform.forward;
        Collider[] hitTargets = Physics.OverlapSphere(attackOffset, blackboard.monsterData.attackDamageRadius);

        if(hitTargets.Length == 0){
            transform.rotation = Quaternion.Slerp(transform.rotation, blackboard.player.transform.rotation, Time.deltaTime * 5.0f);
        }else{
            foreach(var target in  hitTargets)
            {
                if (target.CompareTag("blackboard.player"))
                {
                    target.GetComponentInChildren<PlayerController>().PlayerTakeDamage(blackboard.monsterData.attackPower);
                }
            }
        }
        StartCoroutine(Coroutine_AttackCoolDown());
    }

    private IEnumerator Coroutine_AttackCoolDown()
    {
        isMonsterAttackCoolDown = true;
        blackboard.animator.SetBool("Walk", false);
        yield return new WaitForSeconds(blackboard.monsterData.attackCooldown);
        isMonsterAttackCoolDown = false;
        blackboard.isAttacking = false;
    }
}