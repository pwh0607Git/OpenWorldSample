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

        if (droppedItem == null || !CheckVaildItem<ItemType>(droppedItem))
            return;

        if(currentItem == null){
            // 해당 공간에 단순히 할당 하면됨
        }else{
            //스왑부분
        }

        // droppedItem.transform.SetParent(transform);
        // droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        // OnChangedItem?.Invoke();
    }

    public void AssignCurrentItem(GameObject item){
        if(item.GetComponentInChildren<ItemDataHandler>() == null) return;
        currentItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector2.zero;
        OnChangedItem?.Invoke();
    }

    public void ClearCurrentItem()
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

    public virtual bool CheckVaildItem<T>(GameObject item, T? validType = null) where T : struct
    {
        ItemData itemData = item.GetComponent<ItemDataHandler>().GetItem;

        if (validType.HasValue)
        {
            return item != null && itemData != null && itemData.itemType.Equals(validType);
        }
        else
        {
            return item != null && itemData != null;
        }
    }
}