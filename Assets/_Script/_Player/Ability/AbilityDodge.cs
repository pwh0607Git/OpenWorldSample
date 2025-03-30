using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ }
    public override void FixedUpdate(){

    }

    public override void Activate()
    {
        PerformDodge();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    
    void PerformDodge(){
        Vector3 direction = player.transform.forward;
        Vector3 targetPosition = player.transform.position + direction * 1f; // 1유닛 전진
        
        PlayAnimation();    
        player.transform.DOJump(targetPosition, 1f, 1, 1f)
        .SetEase(Ease.OutQuad)
        .OnComplete(() => Debug.Log("회피 완료!"));
    }
    
    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("Dodge", 0.02f, 0, 0f);
    }
}