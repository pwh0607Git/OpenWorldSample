using System;

[Flags]
public enum AbilityFlag
{
    None = 0,
    Move = 1 << 0,      // 0001
    Jump = 1 << 1,      // 0010
    Attack = 1 << 2,    // 0100
    Damaged = 1 << 3,
    Dodge = 1 << 4,     // 0001, 0000
}

[Serializable]
public abstract class Ability{
    public virtual void Activate() { }
    public virtual void Deactivate() { }
    public virtual void FixedUpdate() { }
    public virtual void Update(){ }
}

public abstract class Ability<T> : Ability where T : class{
    public T data;
    public abstract AbilityFlag Flag{get;}
    protected PlayerController1 player;

    public Ability(T data, PlayerController1 player, float cooldown = 0f){
        this.data = data;
        this.player = player;
    }
}