public interface IStateEffect
{
    void Apply(PlayerState state);
}

public class HealEffect : IStateEffect
{
    private int amount;
    public HealEffect(int amount) => this.amount = amount;
    
    public void Apply(PlayerState state) => state.Heal(amount);
}

public class ManaRestoreEffect : IStateEffect
{
    private int amount;
    public ManaRestoreEffect(int amount) => this.amount = amount;
    
    public void Apply(PlayerState state) => state.RestoreMana(amount);
}

// public class DamageEffect : IStateEffect
// {
//     private int damage;
    
//     public DamageEffect(int damage) => this.damage = damage;
    
//     public void Apply(PlayerState state) => state.TakeDamage(damage);
// }

// public class StunEffect : IStateEffect
// {
//     private float duration;

//     public StunEffect(float duration) => this.duration = duration;

//     public void Apply(PlayerState state) => state.ApplyStun(duration);
// }

public class AttackUpBuffEffect : IStateEffect
{
    private float amount;
    public AttackUpBuffEffect(float amount) => this.amount = amount;
    
    public void Apply(PlayerState state) => state.ApplyBonus(new State());          //수정 부분.
}

public static class EffectFactory
{
    public static IStateEffect CreateEffect(Consumable consumable)
    {
        ConsumableData data = consumable.data as ConsumableData;
        return data.subType switch
        {
            ConsumableType.HP => new HealEffect((int)data.value),
            ConsumableType.MP => new ManaRestoreEffect((int)data.value),
            ConsumableType.Attackup => new AttackUpBuffEffect(data.value),
            _ => null
        };
    }

    public static IStateEffect CreateEffect(EffectType type){
        return type switch{
            // EffectType.Damage => new 
            _ => null
        };
    }
}

public enum EffectType{
    Damage, Stun
}