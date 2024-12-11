using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType equipmentType;

    public override void Update()
    {
        base.Update();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag.gameObject;

        if (CheckVaildItem(droppedItem))
        {
            ItemData itemData = droppedItem.GetComponent<ItemDataSC>().GetItem;
            if (checkEquipmentItem(itemData))
            {
                droppedItem.transform.SetParent(transform);
                droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                currentItem = droppedItem;
            }
        }
    }

    bool checkEquipmentItem(ItemData itemData)
    {
        if (itemData != null && itemData is Equipment equipment)
        {
            if (equipment.subType == equipmentType) return true;
            else return false;
        }
        else
        {
            return false;
        }
    }
}