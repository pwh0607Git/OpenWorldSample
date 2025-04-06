public class AbilityDamaged : Ability<PlayerState>
{
    float abilityDuration;
    public override void Activate() {
        TakeDamage();
    }

    public override void Deactivate() { }
    
    public override void FixedUpdate() { }

    public AbilityDamaged(PlayerState data, PlayerController1 player) : base(data,player){
        float animationSpeed = player.animator.GetFloat("DAMAGEDSPEED");
        abilityDuration = player.animator.GetAnimationClipLength("TakeDamage");
    }

    public void TakeDamage(){
        //애니메이션 만 수행.
        PlayAnimation();
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("TakeDamage", 0.02f, 0, 0f);
    }
}
