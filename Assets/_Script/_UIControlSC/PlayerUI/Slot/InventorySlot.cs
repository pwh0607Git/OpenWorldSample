using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : DragAndDropSlot, IDropHandler
{
    public int index;
    public event Action<SlotData<int>> OnSlotUpdated;

    #region UIITemEventHandler R 
    public override void SetItem(GameObject itemIcon, bool f = false)
    {
        base.SetItem(itemIcon);
        
        if(!f) return;
        Debug.Log("Set Inventory item...");
        Item item = assignedItem.GetComponent<ItemIcon>().item;
        OnSlotUpdated?.Invoke(new SlotData<int>(index, item, item.count));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();
        
        if(!f) return;
        
        Debug.Log("Clear Inventory item...");
        OnSlotUpdated?.Invoke(new SlotData<int>(index, null));
        
    }
    #endregion
    
    public override bool CheckVaildItem(GameObject itemIcon)
    {
        return base.CheckVaildItem(itemIcon);
    }
}