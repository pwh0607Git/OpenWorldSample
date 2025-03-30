public class AbilityDamaged : Ability<PlayerState>
{
    public override void Activate() { }
    public override void Deactivate() { }
    public override void FixedUpdate() { }

    public AbilityDamaged(PlayerState data, PlayerController1 player) : base(data,player){ }

    public void TakeDamage(){
        //애니메이션 만 수행.
        PlayAnimation();
    }

    private void PlayAnimation(){
        player.animator.CrossFadeInFixedTime("TakeDamage", 0.02f, 0, 0f);
    }
}
