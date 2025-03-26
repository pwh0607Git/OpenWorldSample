public class AbilityDamaged : Ability<PlayerState>
{
    public override void Activate() { }
    public override void Deactivate() { }
    public override void FixedUpdate() { }

    public AbilityDamaged(PlayerState data, PlayerController1 player) : base(data,player){ }

    public void TakeDamage(){
        //애니메이션 만 수행.
        player.animator?.CrossFadeInFixedTime("JUMPUP", 0.6f, 0, 0f);
    }
}
