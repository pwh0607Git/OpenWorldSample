using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : DragAndDropSlot, IDropHandler
{
    public int index;
    public event Action<SlotData<int>> OnSlotUpdated;
    
    public override bool CheckVaildItem(GameObject item)
    {
        return base.CheckVaildItem(item);
    }

    #region UIITemEventHandler R 
    public override void SetItem(GameObject item, bool f = false)
    {
        base.SetItem(item);
        
        if(!f) return;
        ItemData itemData = assignedItem.GetComponentInChildren<ItemDataHandler>()?.GetItem;
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