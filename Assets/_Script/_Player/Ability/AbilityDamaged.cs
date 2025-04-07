using DG.Tweening;

public class AbilityDamaged : Ability<PlayerState>
{
    float abilityDuration;
    public override void Activate() {
        TakeDamage();
    }

    public override void Deactivate() { 
        player.currentActivatedAbilities.Remove(AbilityFlag.Damaged);
    }

    public AbilityDamaged(PlayerState data, PlayerController1 player) : base(data,player){
        float animationSpeed = player.animator.GetFloat("DAMAGEDSPEED");
        abilityDuration = player.animator.GetAnimationClipLength("TakeDamage") / animationSpeed;
    }

    public void TakeDamage(){
        //애니메이션 만 수행.
        if(player.currentActivatedAbilities.HasAny(AbilityFlag.Damaged)) return;

        player.currentActivatedAbilities.Add(AbilityFlag.Damaged);

        DG.Tweening.Sequence damageSeq = DOTween.Sequence(player.gameObject);
        damageSeq.AppendCallback(() => PlayAnimation());
        damageSeq.AppendInterval(abilityDuration);
        damageSeq.OnComplete(()=>Deactivate());
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("TakeDamage", 0.02f, 0, 0f);
    }
}
