using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

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
    
    public event Action OnConsumableUsed;           //소비아이템 사용/획득 콜백.

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
            Debug.Log("해당 아이템이 없습니다.");
            return;
        }

        State state = PlayerController.player.myState;
        state.UesConsumable(this);
        consumableCount--;
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