using UnityEngine;

/*
    공격 수행중 canCombo가가
*/
public class AbilityAttack : Ability<AbilityAttackData>
{
    public override AbilityFlag Flag => AbilityFlag.Attack;
    private float slash1Duration, slash2Duration;
    float elapsed = 0f;
    int comboIndex = 0;
    private bool isPerforming = false;

    public AbilityAttack(AbilityAttackData data, PlayerController1 player) : base(data, player) { 
        slash1Duration = player.animator.GetAnimationClipLength("Slash1") / player.animator.GetFloat("SLASH1SPEED");
        slash2Duration = player.animator.GetAnimationClipLength("Slash2") / player.animator.GetFloat("SLASH2SPEED");
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
        if(Input.GetMouseButtonDown(0)) PerformAttack();

        if(isPerforming){
            elapsed += Time.deltaTime;

            //1번 슬래시
            if(comboIndex < 1){

            }
            else{       //2번 슬래시
                if(elapsed >= slash1Duration){
                    Deactivate();

                    elapsed = 0f;
                    isPerforming = false;
                }
            }
        }
    }

    void PerformAttack(){                
        if(comboIndex >= 1) return;

        if(isPerforming){    
            if(elapsed > slash1Duration / 2){
                if(comboIndex >= 1) return;
                comboIndex++;
                PlayAnimation();
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