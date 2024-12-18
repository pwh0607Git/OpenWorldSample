using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType equipmentType;

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (CheckEquipmentItem(droppedItem))
        {
            base.OnDrop(eventData);
        }
    }

    public bool CheckEquipmentItem(GameObject item)
    {
        return base.CheckVaildItem<ItemType>(item, ItemType.Equipment);
    }
}