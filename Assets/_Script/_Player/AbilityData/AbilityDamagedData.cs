using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Damaged")]
public class AbilityDamagedData : AbilityData
{
    public override AbilityFlag flag => AbilityFlag.Damaged;
    public float duration;
    public override Ability CreateAbility(PlayerController1 owner) => new AbilityDamaged(this, owner);
}
