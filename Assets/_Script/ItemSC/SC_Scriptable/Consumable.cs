using System;
using UnityEngine;

public enum ConsumableType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumable : ItemData
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

    public override void Use()
    {
        if (count <= 0)
        {
            return;
        }

        count--;
        State state = PlayerController.player.myState;
        state.UesConsumable(this);
        OnConsumableUsed?.Invoke();
    }

    public void GetThisItem()
    {
        count++;
        OnConsumableUsed?.Invoke();
    }

    public void ThrowThisItem()
    {
        count = 0;
        OnConsumableUsed?.Invoke();
    }
}