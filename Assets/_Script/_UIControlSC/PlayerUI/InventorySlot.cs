using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : DragAndDropSlot, IDropHandler
{
    public int index;
    public event Action<SlotData<int>> OnSlotUpdated;
    
    public override bool CheckVaildItem(GameObject itemIcon)
    {
        return base.CheckVaildItem(itemIcon);
    }

    #region UIITemEventHandler R 
    public override void SetItem(GameObject itemIcon, bool f = false)
    {
        base.SetItem(itemIcon);
        
        if(!f) return;
        Item item = assignedItem.GetComponentInChildren<Item>();
        OnSlotUpdated?.Invoke(new SlotData<int>(index, item.data));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();
        
        if(!f) return;
        OnSlotUpdated?.Invoke(new SlotData<int>(index, null));
        
    }
    #endregion
}