using System;
using UnityEditor;
using UnityEngine;

public enum ConsumableType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Items/Consumable")]
public class ConsumableData : ItemData
{
    public ConsumableType subType;
    public bool isPresetting { get; set; }
    private int count;
    
    public event Action OnConsumableUsed;

    public ItemData GetItemData() => this;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
        count = 1;
        isPresetting = false;
    }

    public int GetConsumableCount() { return count; }

    // public override void Use()
    // {
    //     if (count <= 0) return;

    //     count--;
    //     State state = PlayerController.player.myState;
    //     state.UesConsumable(this);
    //     OnConsumableUsed?.Invoke();
    // }

    // public void GetThisItem()
    // {
    //     count++;
    //     OnConsumableUsed?.Invoke();
    // }

    // public void ThrowThisItem()
    // {
    //     count = 0;
    //     OnConsumableUsed?.Invoke();
    // }
}

// 추수 인스턴스화하여 사용 할 예정.
public class ConsumableItem {
    private ConsumableData data;
    private int count;

    public ConsumableItem(ConsumableData data, int count = 1){
        this.data = data;
        this.count = count;
    }

    public void Use(PlayerState state){
        if (count <= 0) return;
        count--;

        IStateEffect effect = EffectFactory.CreateEffect(data.subType, data.value);
        effect?.Apply(state);

        // OnConsumableUsed?.Invoke();
    }

    private IStateEffect GetEffect()
    {
        return data.subType switch
        {
            ConsumableType.HP => new HealEffect((int)data.value),
            ConsumableType.MP => new ManaRestoreEffect((int)data.value),
            _ => null
        };
    }
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