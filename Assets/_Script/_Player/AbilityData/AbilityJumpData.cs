using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Move")]
public class AbilityJumpData : AbilityData
{
    public override AbilityFlag flag => AbilityFlag.Jump;
    public float duration = 0.5f;
    public float jumpForce = 10f;
    public AnimationCurve jumpCurve;
    public override Ability CreateAbility(PlayerController1 owner) => new AbilityJump(this, owner);
}