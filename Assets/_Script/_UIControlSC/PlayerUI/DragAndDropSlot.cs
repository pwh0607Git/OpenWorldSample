using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentItem {get; private set;}
    
    public event Action OnChangedItem;

    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;
        if (droppedItem != null && droppedItem.GetComponent<ItemIconController>() != null && CheckVaildItem<ItemType>(droppedItem))
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            OnChangedItem?.Invoke();
        }
    }

    public virtual bool CheckVaildItem<T>(GameObject item, T? validType = null) where T : struct
    {
        ItemData itemData = item.GetComponent<ItemDataSC>().GetItem;

        if (validType.HasValue)
        {
            return item != null && itemData != null && itemData.itemType.Equals(validType);
        }
        else
        {
            return item != null && itemData != null;
        }
    }

    public void AssignCurrentItem(GameObject item)
    {
        currentItem = item;
    }

    public void ClearCurrentItem()
    {
        currentItem = null;
        Destroy(transform.GetComponentInChildren<ItemIconController>().gameObject);
    }
}