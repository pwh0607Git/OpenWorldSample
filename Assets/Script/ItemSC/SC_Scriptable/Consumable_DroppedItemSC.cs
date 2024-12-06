using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable_DroppedItemSC : ItemDataSC
{
    [SerializeField]
    private Consumable consumableItem;

    public override ItemData GetItem => consumableItem;
}