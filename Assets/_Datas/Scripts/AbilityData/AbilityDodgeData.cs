using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Ability Dodge")]
public abstract class AbilityDodgeData : AbilityData
{    
    public override AbilityFlag flag => AbilityFlag.Dodge;
    public float duration;
    public float height;
    public float coolDown;
}