using System;

public abstract class Item
{
    protected ItemData data;

    public ItemData GetData { get; }
    public int Count { get; set; }

    protected Item(ItemData data, int count = 1)
    {
        this.data = data;
        this.Count = count;
    }

    public abstract void Use(PlayerState state);
}

public class Consumable : Item
{
    public Consumable(ConsumableData data, int count = 1) : base(data, count) { }
    public event Action OnConsumableUsed;
    public override void Use(PlayerState state)
    {
        if (Count <= 0) return;
        Count--;
        IStateEffect effect = EffectFactory.CreateEffect(((ConsumableData)data).subType, data.value);
        effect?.Apply(state);
    }

    public void SubscribeToUseEvent(Action callback)
    {
        OnConsumableUsed += callback;
    }
}

public class Equipment : Item {
    public Equipment(EquipmentData data, int count = 1) :base(data,count){ }
    public override void Use(PlayerState state)
    {
        if (Count <= 0) return;
        Count--;
        IStateEffect effect = EffectFactory.CreateEffect(((ConsumableData)data).subType, data.value);
        effect?.Apply(state);
    }
}