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
    private int consumableCount;
    
    public event Action OnConsumableUsed;           //�Һ������ ���/ȹ�� �ݹ�.

    public void Init()
    {

    }

    public ItemData GetItemData() => this;

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
        consumableCount = 1;
        isPresetting = false;
    }

    public int GetConsumableCount() { return consumableCount; }

    public override void Use()
    {
        if (consumableCount <= 0)
        {
            return;
        }

        consumableCount--;
        State state = PlayerController.player.myState;
        state.UesConsumable(this);
        OnConsumableUsed?.Invoke();
    }

    public void GetThisItem()
    {
        consumableCount++;
        OnConsumableUsed?.Invoke();
    }

    public void ThrowThisItem()
    {
        consumableCount = 0;
        OnConsumableUsed?.Invoke();
    }
}