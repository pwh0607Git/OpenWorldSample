using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityData : ScriptableObject
{
    public abstract AbilityFlag flag {get;}
    public abstract Ability CreateAbilty(PlayerController1 controller);
}