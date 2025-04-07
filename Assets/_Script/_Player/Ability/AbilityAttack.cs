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
    float elapsed = 0f;

    public AbilityAttack(PlayerState data, PlayerController1 player, AttackArea area) : base(data, player) { 
        this.area = area;
        area.OnMonsterListChanged += UpdateAttackableMonsters;
    
        float animationSpeed = player.animator.GetFloat("SLASH1SPEED");
        animationDuration = player.animator.GetAnimationClipLength("Slash1") / animationSpeed;
    }

    public override void Activate()
    {
        PerformAttack();
    }

    public override void Deactivate()
    {
        player.currentActivatedAbilities.Remove(AbilityFlag.Attack);
        player.animator.SetFloat("COMBOINDEX", 0f);
        elapsed = 0f;
    }
    
    public override void Update(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            Activate();
        }

        if(isPerforming){
            elapsed += Time.deltaTime;

            if(elapsed <= animationDuration){           //시간전에 재 클릭시.
                if(player.animator.GetFloat("COMBOINDEX") == 0){
                    
                }
            }else{
                Deactivate();
            }
        }
    }

    //콤보 어택 link
    // https://daekyoulibrary.tistory.com/entry/Charon-7-%EB%AC%B4%EA%B8%B0-%EA%B8%B0%EB%B3%B8-3%ED%83%80-%EC%BD%A4%EB%B3%B4-%EA%B3%B5%EA%B2%A9-%EA%B5%AC%ED%98%84%ED%95%98%EA%B8%B0-feat-%EC%83%81%ED%83%9C-%ED%8C%A8%ED%84%B4

    void PerformAttack(){
        float comoboIndex = player.animator.GetFloat("COMBOINDEX");

        if(comoboIndex <= 0) {
            player.animator.SetFloat("COMBOINDEX", 1f);
        }

        if(player.currentActivatedAbilities.HasAny(AbilityFlag.Attack)) return;

        player.currentActivatedAbilities.Add(AbilityFlag.Attack);
        player.animator.SetFloat("MOVESPEED", 0.01f);
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