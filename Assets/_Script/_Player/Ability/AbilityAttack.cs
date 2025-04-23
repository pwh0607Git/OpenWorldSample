using UnityEngine;

/*
    공격 수행중 canCombo가가
*/
public class AbilityAttack : Ability<AbilityAttackData>
{
    public override AbilityFlag Flag => AbilityFlag.Attack;
    private float animationDuration;
    float elapsed = 0f;
    int comboIndex = 0;
    private bool isPerforming = false;

    public AbilityAttack(AbilityAttackData data, PlayerController1 player) : base(data, player) { 
        animationDuration = player.animator.GetAnimationClipLength("Slash1") / player.animator.GetFloat("SLASH1SPEED");
    }

    public override void Activate()
    {
        player.abilityController.Activate(Flag, true);
        comboIndex = 0;
    }

    public override void Deactivate()
    {
        Debug.Log("콤보 어택 종료...");
        comboIndex = 0;
        // player.abilityController.Activate(AbilityFlag.None, true);
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
            }
        }
    }

    void PerformAttack(){
        if(isPerforming){    
            if(elapsed > animationDuration / 2){
                if(comboIndex >= 1) return;
                comboIndex++;
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