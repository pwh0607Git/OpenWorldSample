using System.Collections.Generic;
using UnityEngine;

/*
    공격 수행중 canCombo가가
*/
public class AbilityAttack : Ability<PlayerState>
{
    private HashSet<GameObject> attackableMonsters = new();
    private AttackArea area;
    
    private float animationDuration;
    private bool isPerforming = false;
    private bool comboQueued = false;
    private int comboIndex = 0;
    
    float elapsed = 0f;

    public AbilityAttack(PlayerState data, PlayerController1 player, AttackArea area) : base(data, player) { 
        this.area = area;
        area.OnMonsterListChanged += UpdateAttackableMonsters;
    
        float animationSpeed = player.animator.GetFloat("AttackCombo");
        animationDuration = player.animator.GetAnimationClipLength("Slash1") / 1;
        Debug.Log($"Slash 1 : {animationSpeed},   {animationDuration}");
    }

    public override void Activate()
    {
        PerformAttack();
    }

    public override void Deactivate()
    {
        player.currentActivatedAbilities.Remove(AbilityFlag.Attack);
    }
    
    public override void Update(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Activate();
        }

        if(isPerforming){
            elapsed += Time.deltaTime;

            if(elapsed > animationDuration){
                Deactivate();
                isPerforming = false;
            }
        }
    }

    void PerformAttack(){
        if(player.currentActivatedAbilities.HasAny(AbilityFlag.Attack)) return;

        player.animator.SetFloat("MOVESPEED", 0.01f);
        player.currentActivatedAbilities.Add(AbilityFlag.Attack);
        isPerforming = true;
        PlayAnimation();
        
        // if(attackableMonsters.Count <= 0) return;

        foreach (var monster in attackableMonsters)
        {
            Debug.Log($"Attacking {monster.name}");
            monster.GetComponent<MonsterController>().TakeDamage(10);       //test
        }     
    }

    void UpdateAttackableMonsters(HashSet<GameObject> monsters){
        attackableMonsters = monsters;
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("AttackCombo", 0.02f, 0, 0f);
    }
}