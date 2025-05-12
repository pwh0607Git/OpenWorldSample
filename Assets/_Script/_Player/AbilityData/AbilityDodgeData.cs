using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Dodge")]
public class AbilityDodgeData : AbilityData
{    
    public override AbilityFlag flag => AbilityFlag.Dodge;
    public float height;
    public float speed;
    
    public override Ability CreateAbility(PlayerController1 player) => new AbilityDodge(this, player);
}