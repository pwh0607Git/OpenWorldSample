using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public abstract AbilityFlag flag {get;}

    public AbilityFlag isolateFlags;

    public abstract Ability CreateAbility( PlayerController1 player );
}