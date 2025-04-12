using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    float abilityDuration;
    private bool isPerforming = false;          //중복 체크.
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ 
        float animationSpeed = player.animator.GetFloat("DODGESPEED");
        abilityDuration = player.animator.GetAnimationClipLength("Dodge") / animationSpeed;

    }
    public override void Activate()
    {
        if(isPerforming) return;
        PerformDodge();
    }

    public override void Deactivate()
    {
        isPerforming = false;
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            PerformDodge();
    }
    
    void PerformDodge(){
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