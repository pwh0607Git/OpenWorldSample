using UnityEngine;

public class AbilityMoveData : AbilityData
{
    public override AbilityFlag flag => AbilityFlag.Move;
    
    public override Ability CreateAbility(PlayerController1 owner) => new AbilityMove(this, owner);
}