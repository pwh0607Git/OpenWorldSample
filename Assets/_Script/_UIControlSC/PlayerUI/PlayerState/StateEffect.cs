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

public static class EffectFactory
{
    public static IStateEffect CreateEffect(ConsumableType type, float value)
    {
        return type switch
        {
            ConsumableType.HP => new HealEffect((int)value),
            ConsumableType.MP => new ManaRestoreEffect((int)value),
            _ => null
        };
    }
}