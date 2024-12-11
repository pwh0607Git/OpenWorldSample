using Gentleland.Utils.SteampunkUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class InventorySlot : DragAndDropSlot
{
    public override void Update()
    {
        base.Update();
    }

    public override void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //드래그 상태인 item 참조.

        if (CheckVaildItem(droppedItem))
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            currentItem = droppedItem;
        }
    }
}