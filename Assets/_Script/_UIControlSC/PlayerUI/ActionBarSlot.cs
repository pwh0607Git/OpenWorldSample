using UnityEngine;
using System;

public class ActionBarSlot : DragAndDropSlot{
    public KeyCode assignedKey;
    public event Action<SlotData<KeyCode>> OnSlotUpdated;

    public override bool CheckVaildItem(GameObject itemIcon){
        Item itemData = itemIcon.GetComponentInChildren<Item>();
        if(itemData == null) return false;
        return itemData is Consumable consumable;
    }
    
    #region UIITemEventHandler R 
    public override void SetItem(GameObject itemIcon, bool f = false)
    {
        base.SetItem(itemIcon);
        
        if(!f) return;
        Debug.Log("Slot Set Item...");
        Item item = itemIcon.GetComponentInChildren<Item>();
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, item.data));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();

        if(!f) return;
        Debug.Log("Slot Clear Item...");
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, null));
    }
    #endregion
}