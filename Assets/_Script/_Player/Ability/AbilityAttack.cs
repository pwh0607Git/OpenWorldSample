using System.Collections.Generic;
using UnityEngine;

public class AbilityAttack : Ability<PlayerState>
{
    private HashSet<GameObject> attackableMonsters = new();
    private AttackArea area;
    private float attackCoolDown = 0.5f;
    private float lastAttackTime = 0f;
    public AbilityAttack(PlayerState data, PlayerController1 player, AttackArea area) : base(data, player) { 
        this.area = area;
        area.OnMonsterListChanged += UpdateAttackableMonsters;
    }

    public override void Activate()
    {
        PerformAttack();
    }
    
    void PerformAttack(){
        if (Time.time - lastAttackTime < attackCoolDown) return;
        Debug.Log("Attack!");

        if(attackableMonsters.Count <= 0) return;

        foreach (var monster in attackableMonsters)
        {
            Debug.Log($"Attacking {monster.name}");
        }
        
    }

    void UpdateAttackableMonsters(HashSet<GameObject> monsters){
        attackableMonsters = monsters;
    }
}