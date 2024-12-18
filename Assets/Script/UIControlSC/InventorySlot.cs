using Gentleland.Utils.SteampunkUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : DragAndDropSlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (CheckVaildItem<ItemType>(droppedItem))
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }
}