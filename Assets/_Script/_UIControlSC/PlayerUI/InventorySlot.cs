using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : DragAndDropSlot, IDropHandler
{
    public event Action<SlotData<int>> OnSlotUpdated;
    public int index;
    public override bool CheckVaildItem(GameObject item)
    {
        return base.CheckVaildItem(item);
    }

    #region UIITemEventHandler R 
    public override void SetItem(GameObject item, bool f = false)
    {
        base.SetItem(item);
        
        ItemData itemData = assignedItem.GetComponentInChildren<ItemDataHandler>()?.GetItem;
        if(!f) return;
        OnSlotUpdated?.Invoke(new SlotData<int>(index, itemData));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();
        
        if(!f) return;
        OnSlotUpdated?.Invoke(new SlotData<int>(index, null));
        
    }
    #endregion
}