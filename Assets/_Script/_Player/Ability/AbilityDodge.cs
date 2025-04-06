using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    float abilityDuration;
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ 
        float animationSpeed = player.animator.GetFloat("DODGESPEED");
        abilityDuration = player.animator.GetAnimationClipLength("Dodge") / animationSpeed;
    }
    public override void Activate()
    {
        PerformDodge();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    public override void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && !player.currentActivatedAbilities.Has(AbilityFlag.Dodge)){
            Activate();
        }
    }
    
    void PerformDodge(){
        Vector3 direction = player.transform.forward;
        Vector3 targetPosition = player.transform.position + direction * 1f;            // 1유닛 전진
        
        player.currentActivatedAbilities.Add(AbilityFlag.Dodge,null);

        PlayAnimation();

        player.transform.DOJump(targetPosition, 0.5f, 1, abilityDuration).SetEase(Ease.Unset)
        .OnComplete(()=>{
            player.currentActivatedAbilities.Remove(AbilityFlag.Dodge,null);
        });
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("Dodge", 0.02f, 0, 0f);
    }
}