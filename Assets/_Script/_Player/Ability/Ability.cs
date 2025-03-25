using System;
using UnityEngine;

[Flags]
public enum AbilityFlag
{
    None = 0,
    Move = 1 << 0,      // 0001
    Jump = 1 << 1,      // 0010
    Dodge = 1 << 2,     // 0100
    Attack = 1 << 3,    // 1000
}

[Serializable]
public abstract class Ability{
    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void FixedUpdate() { }
}

public abstract class Ability<T> : Ability where T : class{
    public T data;
    protected PlayerController1 player;
    public Ability(T data, PlayerController1 player){
        this.data = data;
        this.player = player;
    }
}