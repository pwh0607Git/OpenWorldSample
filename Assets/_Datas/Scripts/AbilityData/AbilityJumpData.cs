using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Move")]
public class AbilityJumpData : AbilityData
{
    public float duration = 0.5f;
    public float jumpForce = 10f;
    public AnimationCurve jumpCurve;
    public override AbilityFlag flag => AbilityFlag.Jump;
    public override Ability CreateAbilty(PlayerController1 player) => new AbilityJump(this, player);
}
