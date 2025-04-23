using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Attack")]
public class AbilityAttackData : AbilityData
{
    public override AbilityFlag flag => AbilityFlag.Attack;
    
    public override Ability CreateAbility(PlayerController1 owner) => new AbilityAttack(this, owner);
}
