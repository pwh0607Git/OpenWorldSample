using System.Collections.Generic;
using UnityEngine;

public class AbilityComboAttack : Ability<PlayerState>
{
    private HashSet<GameObject> attackableMonsters = new();

    private float animationDuration;
    public AbilityComboAttack(PlayerState data, PlayerController1 player) : base(data, player) { 
        animationDuration = player.animator.GetAnimationClipLength("Slash2") / player.animator.GetFloat("SLASH2SPEED");
    }

    public override void Activate()
    {
        PerformAttack();
    }

    public override void Deactivate()
    {

    }
    
    public override void Update(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Activate();
        }
    }

    void PerformAttack(){
        // if(player.currentActivatedAbilities.HasAny(AbilityFlag.Attack)) return;

        // 중복 호출 제어 코드
        // ...

        player.animator.SetFloat("MOVESPEED", 0.01f);
        PlayAnimation();
        
        if(attackableMonsters.Count <= 0) return;

        foreach (var monster in attackableMonsters)
        {
            monster.GetComponent<MonsterController>().TakeDamage(10);       //test
        }     
    }

    void UpdateAttackableMonsters(HashSet<GameObject> monsters){

    }

    private void PlayAnimation(){
    }
}