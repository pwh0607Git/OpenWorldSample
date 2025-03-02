using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    private GameObject assignedItem;
    public void OnDrop(PointerEventData eventData){
        GameObject droppedItem = eventData.pointerDrag;
        UIItemEventHandler.OnChangedSlot(this, droppedItem);
    }

    public void SetItem(GameObject item){
        assignedItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector2.zero;
    }
    public void ClearSlot(){
        assignedItem = null;
    }
    public GameObject GetItem(){
        return assignedItem;
    }
    public virtual bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        return itemData != null;
    }
}