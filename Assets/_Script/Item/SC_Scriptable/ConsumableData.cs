using UnityEngine;

public enum ConsumableType
{
    HP,
    MP,
    Attackup
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Items/ConsumableData")]
public class ConsumableData : ItemData
{
    public ConsumableType subType;
}

