using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    protected GameObject currentItem;

    public GameObject GetCurrentItem() { return currentItem; }

    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem != null && droppedItem.GetComponent<ItemIconController>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
    }

    //아이템 유효성 검사
    public virtual bool CheckVaildItem<T>(GameObject item, T? validType = null) where T : struct
    {
        ItemData itemData = item.GetComponent<ItemDataSC>().GetItem;

        if (validType.HasValue)
        {
            return (item != null && itemData != null && itemData.itemType.Equals(validType));
        }
        else
        {
            return (item != null && itemData != null);
        }
    }

    public void AssignCurrentItem(GameObject item)
    {
        currentItem = item;
    }

    public void CleanCurrentItem()
    {
        currentItem = null;
    }
}