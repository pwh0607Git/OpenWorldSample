using System;
using UnityEngine;

public interface IStateEffect
{
    void Apply(PlayerState state);
    void Remove(PlayerState state);
    EffectData Data{get; set;}
}

public interface IAdditiveEffect{
    void Apply();
    void Remove();
    EffectData Data{get; set;}
}

public class HealEffect : IStateEffect
{
    private EffectData data;
    public HealEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.Heal((int)data.value);
    
    public void Remove(PlayerState state) { }   
    public EffectData Data {get => data; set => data = value;}
}

public class ManaRestoreEffect : IStateEffect
{
    private EffectData data;
    public ManaRestoreEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.RestoreMana((int)data.value);
    public void Remove(PlayerState state) { }
    

    public EffectData Data {get => data; set => data = value;}
}

public class DamageEffect : IStateEffect
{
    private EffectData data;
    public DamageEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.ApplyDamage((int)data.value);
    public void Remove(PlayerState state) { }

    public EffectData Data {get => data; set => data = value;}
}

public class AttackUpBuffEffect : IStateEffect
{
    public EffectData data;
    public AttackUpBuffEffect(EffectData effectData){
        data = effectData;
    }
    
    public void Apply(PlayerState state) => state.ApplyBonus(new State(0, 0, (int)data.value, 0, 0));
    public void Remove(PlayerState state) => state.RemoveBonus(new State(new State(0, 0, (int)data.value, 0, 0)));

    public EffectData Data {get => data; set => data = value;}
}

public class StunEffect : IAdditiveEffect{
    private EffectData data;
    public StunEffect(EffectData data) => this.data = data;
    public void Apply(){}
    public void Remove(){}

    public EffectData Data {get => data; set => data = value;}
}

public class SlowEffect : IAdditiveEffect{
    private EffectData data;
    public StunEffect(EffectData data) => this.data = data;
    public void Apply(){}
    public void Remove(){}

    public EffectData Data {get => data; set => data = value;}
}

public static class EffectFactory
{
    public static IStateEffect CreateEffect(Consumable consumable)
    {
        ConsumableData consumableData = consumable.data as ConsumableData;
        EffectData effectData;

        if(consumableData is BuffConsumableData buffData)
            effectData = new EffectData(buffData.value, buffData.duration, buffData.icon);
        else
            effectData = new EffectData(consumableData.value, 0, consumableData.icon);

        return consumableData.subType switch
        {
            ConsumableType.HP => new HealEffect(effectData),
            ConsumableType.MP => new ManaRestoreEffect(effectData),
            ConsumableType.Attackup => new AttackUpBuffEffect(effectData),
            _ => null
        };
    }

    //Additive
    public static IAdditiveEffect CreateEffect(EffectType type, int value, float duration){
        return type switch{
            EffectType.Stun => new StunEffect(new EffectData(value, duration)),
            EffectType.Slow => new SlowEffect(new EffectData(value, duration)),
            _ => null
        };
    }
}

[Serializable]
public enum EffectType{
    Slow, Stun
}

public class EffectData{
    public float value;
    public float duration;
    public Sprite icon;

    public EffectData(float value, float duration = 0, Sprite icon = null){
        this.value = value;
        this.duration = duration;
        this.icon = icon;
    }
}