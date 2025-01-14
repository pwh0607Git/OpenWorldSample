using UnityEngine;

public class Consumable_DroppedItemSC : ItemDataSC
{
    [SerializeField]
    private Consumable consumableItem;

    public override ItemData GetItem => consumableItem;
}