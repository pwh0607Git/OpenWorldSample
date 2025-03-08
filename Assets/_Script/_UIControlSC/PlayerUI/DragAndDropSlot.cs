using UnityEngine;
using UnityEngine.EventSystems;

public abstract class DragAndDropSlot : MonoBehaviour, IDropHandler
{
    protected GameObject assignedItem;
    public void OnDrop(PointerEventData eventData){
        GameObject droppedItem = eventData.pointerDrag;
        UIItemEventHandler.OnChangedSlot(this, droppedItem);
    }
    public virtual void SetItem(GameObject item, bool f = false)
    {
        assignedItem = item;
        item.transform.SetParent(transform);
        item.transform.localPosition = Vector2.zero;
    }

    public virtual void ClearSlot(bool f = false)
    {
        ItemIcon iconController = GetComponentInChildren<ItemIcon>();
        
        if (iconController != null)
            Destroy(iconController.gameObject);
        assignedItem = null;
    }
    public GameObject GetItem(){
        return assignedItem;
    }
    public virtual bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<Item>().data;
        return itemData != null;
    }
}