using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumeType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "Consumable", menuName = "Items/Consumable")]
public class Consumable : ItemData
{
    public ConsumeType potionType; // 포션 타입          inspector로 세팅

    private void OnEnable()
    {
        itemType = ItemType.Consumable;
    }

    public override void Use(/*GameObject player*/)
    {
        switch (potionType)
        {
            case ConsumeType.HP:
                Debug.Log("HP 포션 사용!");
                break;
            case ConsumeType.MP:
                Debug.Log("MP 포션 사용!");
                break;
            case ConsumeType.SpeedUp:
                Debug.Log("SpeedUp 포션 사용!");
                break;
        }
    }
}