using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Move")]
public class AbilityMoveData : AbilityData
{
    public float movePerSec = 10f;
    public float rotatePerSec = 5f;
    public override AbilityFlag flag => AbilityFlag.Move;
    public override Ability CreateAbilty(PlayerController1 player) => new AbilityMove(this, player);
}