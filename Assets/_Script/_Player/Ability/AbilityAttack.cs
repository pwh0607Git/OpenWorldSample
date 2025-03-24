using System.Collections.Generic;
using UnityEngine;

public class AbilityAttack : Ability<PlayerState>
{
    private HashSet<GameObject> attackableMonsters = new();
    private float attackCoolDown = 0.5f;
    private float lastAttackTime = 0f;
    public AbilityAttack(PlayerState data, PlayerController1 player) : base(data, player) { }
    public override void Activate()
    {
        PerformAttack();
    }
    void PerformAttack(){
        if (Time.time - lastAttackTime < attackCoolDown) return;

        if(attackableMonsters.Count <= 0) return;

        foreach (var monster in attackableMonsters)
        {
            Debug.Log($"Attacking {monster.name}");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsters.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsters.Remove(other.gameObject);
        }
    }
}
