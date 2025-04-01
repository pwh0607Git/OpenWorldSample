using UnityEngine;
using DG.Tweening;

public class AbilityDodge : Ability<AbilityDodgeData>
{
    float duration;
    public AbilityDodge(AbilityDodgeData data, PlayerController1 player) : base(data, player){ 
        duration = GetAnimationClipLength("Dodge");
    }

    private float GetAnimationClipLength(string clipName){
        foreach (AnimationClip clip in player.animator.runtimeAnimatorController.animationClips){
            if (clip.name == clipName) return clip.length; 
        }
        return 0.3f; // 기본값 (애니메이션을 못 찾았을 경우)
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
        player.transform.DOJump(targetPosition, 0.5f, 1, duration/2).SetEase(Ease.Unset);
    }
    
    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("Dodge", 0.02f, 0, 0f);
    }
}