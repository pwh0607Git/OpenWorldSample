using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment_DroppedItemSC : ItemDataSC
{
    [SerializeField]
    private Equipment equipmentItem;

    public override ItemData GetItem => equipmentItem;
}
