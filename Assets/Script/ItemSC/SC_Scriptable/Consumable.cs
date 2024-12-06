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

    public override void Use(/*GameObject player*/)
    {
        if(consumableCount <= 0)
        {
            Debug.Log("해당 아이템이 없습니다.");
            return;
        }

        switch (subType)
        {
            case ConsumableType.HP:
                Debug.Log("HP 포션 사용!");
            break;
            case ConsumableType.MP:
                Debug.Log("MP 포션 사용!");
            break;
            case ConsumableType.SpeedUp:
                Debug.Log("SpeedUp 포션 사용!");
            break;
        }
        consumableCount--;
        OnConsumableUsed?.Invoke();
    }

    public void GetThisItem()
    {
        consumableCount++;
        Debug.Log($"{consumableCount}");
        OnConsumableUsed?.Invoke();
    }

    public void ThrowThisItem()
    {
        consumableCount = 0;
        Debug.Log($"{subType} 개수 ; {consumableCount}");
        OnConsumableUsed?.Invoke();
    }
}