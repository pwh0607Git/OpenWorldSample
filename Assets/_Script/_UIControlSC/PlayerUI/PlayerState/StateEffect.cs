using System;
using UnityEngine;

public interface IStateEffect
{
    void Apply(PlayerState state);
    void Remove(PlayerState state);
    EffectData GetData();
}

public class HealEffect : IStateEffect
{
    public EffectData data;
    public HealEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.Heal((int)data.value);
    
    public void Remove(PlayerState state) { }   
    public EffectData GetData(){
        return data;
    }
}

public class ManaRestoreEffect : IStateEffect
{
    public EffectData data;
    public ManaRestoreEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.RestoreMana((int)data.value);
    public void Remove(PlayerState state) { }
    
    public EffectData GetData(){
        return data;
    }
}

public class DamageEffect : IStateEffect
{
    public EffectData data{get; private set;}
    public DamageEffect(EffectData data) => this.data = data;
    
    public void Apply(PlayerState state) => state.ApplyDamage((int)data.value);
    public void Remove(PlayerState state) { }
    public EffectData GetData(){
        return data;
    }
}

public class AttackUpBuffEffect : IStateEffect
{
    public EffectData data;
    public AttackUpBuffEffect(EffectData effectData){
        data = effectData;
    }
    
    public void Apply(PlayerState state) => state.ApplyBonus(new State(0, 0, (int)data.value, 0, 0));
    public void Remove(PlayerState state) => state.RemoveBonus(new State(new State(0, 0, (int)data.value, 0, 0)));
    public EffectData GetData(){
        return data;
    }
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

    // public static IStateEffect CreateEffect(EffectType type, int value){
    //     return type switch{
    //         EffectType.Damage => new DamageEffect(value),
    //         _ => null
    //     };
    // }
}

[Serializable]
public enum EffectType{
    Damage, 
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