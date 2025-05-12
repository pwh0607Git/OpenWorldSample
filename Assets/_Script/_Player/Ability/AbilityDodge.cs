using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    public override AbilityFlag Flag => AbilityFlag.Dodge;
    float abilityDuration;
    private bool isPerforming = false;       
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ 
        abilityDuration = player.animator.GetAnimationClipLength("Dodge") / data.speed;
    }

    public override void Activate()
    {
        isPerforming = false;
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
        if(isPerforming) return;

        Debug.Log("회피 시작");
        player.abilityController.Activate(Flag, true);

        Vector3 direction = player.transform.forward;
        Vector3 targetPosition = player.transform.position + direction * 1f;            // 1유닛 전진
        
        isPerforming = true;

        PlayAnimation("Dodge", 0.02f, 0, 0f);
    
        player.transform.DOJump(targetPosition, data.height, 1, abilityDuration).SetEase(Ease.Unset)
        .OnComplete(()=>{
            Debug.Log("회피 종료");
            player.abilityController.RestoreAbilities();
            isPerforming = false;
        });
    }
}