using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    public override AbilityFlag Flag => AbilityFlag.Dodge;
    float abilityDuration;
    private bool isPerforming = false;       
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ 
        float animationSpeed = player.animator.GetFloat("DODGESPEED");
        abilityDuration = player.animator.GetAnimationClipLength("Dodge") / animationSpeed;
    }

    public override void Activate()
    {
        player.abilityController.Activate(Flag, true);
        isPerforming = false;
    }

    public override void Deactivate()
    {
        player.abilityController.Activate(AbilityFlag.None, true);
        isPerforming = false;
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            PerformDodge();
    }
    
    void PerformDodge(){
        if(isPerforming) return;

        Vector3 direction = player.transform.forward;
        Vector3 targetPosition = player.transform.position + direction * 1f;            // 1유닛 전진
        
        isPerforming = true;

        PlayAnimation();

        player.transform.DOJump(targetPosition, 0.5f, 1, abilityDuration).SetEase(Ease.Unset)
        .OnComplete(()=>{
            isPerforming = false;
        });
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("Dodge", 0.02f, 0, 0f);
    }
}