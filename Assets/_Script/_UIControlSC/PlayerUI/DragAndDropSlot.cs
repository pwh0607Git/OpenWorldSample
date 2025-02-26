using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentItem;
    protected event Action OnChangedItem;

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;

        if (droppedItem == null || !IsValidItem(droppedItem)) return;
        HandleItemDrop(droppedItem);
    }

    protected virtual void HandleItemDrop(GameObject item){
        if(currentItem == null){
            AssignCurrentItem(item);
        }
        else{
            SwapItem(item);
        }
    }

    public virtual void AssignCurrentItem(GameObject item){
        if(item.GetComponentInChildren<ItemDataHandler>() == null) return;
        currentItem = item;
        item.transform.SetParent(transform);
        item.GetComponent<ItemIconController>().originalSlot = this;
        item.transform.localPosition = Vector2.zero;
        OnChangedItem?.Invoke();      
    }

    public virtual void ClearCurrentItem()
    {
        if(currentItem == null) return;
        if(transform.childCount > 0 )
            Destroy(transform.GetComponentInChildren<ItemIconController>().gameObject);
        currentItem = null;

        OnChangedItem?.Invoke();
    }

    public void SwapItem(GameObject newItem){
        Transform originalParent = newItem.transform.parent;
        newItem.transform.SetParent(transform);
        currentItem.transform.SetParent(originalParent);

        DragAndDropSlot oldSlot = originalParent.GetComponent<DragAndDropSlot>();
        oldSlot.AssignCurrentItem(currentItem);
        AssignCurrentItem(newItem);
    }

    protected abstract bool IsValidItem(GameObject item);
    // public virtual bool CheckVaildItem<T>(GameObject item, T? validType = null) where T : struct
    // {
    //     ItemData itemData = item.GetComponent<ItemDataHandler>().GetItem;

    //     if (validType.HasValue)
    //     {
    //         return item != null && itemData != null && itemData.itemType.Equals(validType);
    //     }
    //     else
    //     {
    //         return item != null && itemData != null;
    //     }
    // }
}