using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    protected GameObject currentItem;

    public GameObject GetCurrentItem() { return currentItem; }
    public void SetCurrentItem(GameObject newItem) { currentItem = newItem; }

    public virtual void Update()
    {
        if (transform.childCount == 0)
        {
            currentItem = null;
        }
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        GameObject droppedItem = eventData.pointerDrag;                                 //드래그 상태인 item 참조.

        if (droppedItem != null && droppedItem.GetComponent<ItemContoller>() != null)
        {
            droppedItem.transform.SetParent(transform);
            droppedItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            currentItem = droppedItem;
        }
    }

    protected virtual bool CheckVaildItem(GameObject item)
    {
        return (item != null && item.GetComponent<ItemContoller>() != null);
    }
}