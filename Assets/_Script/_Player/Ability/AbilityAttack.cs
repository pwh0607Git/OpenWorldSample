using System.Collections.Generic;
using UnityEngine;

/*
    공격 수행중 canCombo가가
*/
public class AbilityAttack : Ability<PlayerState>
{
    private HashSet<GameObject> attackableMonsters = new();
    private float animationDuration;
    float elapsed = 0f;
    private bool isPerforming = false;
    public AbilityAttack(PlayerState data, PlayerController1 player) : base(data, player) { 
        animationDuration = player.animator.GetAnimationClipLength("Slash1") / player.animator.GetFloat("SLASH1SPEED");
    }

    public override void Activate()
    {

    }

    public override void Deactivate()
    {

    }
    

    public override void Update(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            PerformAttack();
        }

        if(isPerforming){
            elapsed += Time.deltaTime;

            if(elapsed >= animationDuration){
                Deactivate();
                
                elapsed = 0f;   
                isPerforming = false;
                elapsed = 0f;
            }
        }
    }

    void PerformAttack(){
        if(isPerforming){    
            if(elapsed > animationDuration / 2){
                player.animator.SetTrigger("COMBOATTACK");
            }
            return;
        }   

        PlayAnimation();
        isPerforming = true;
    }

    private void PlayAnimation(){
        player.animator.SetTrigger("COMBOATTACK");
    }
}