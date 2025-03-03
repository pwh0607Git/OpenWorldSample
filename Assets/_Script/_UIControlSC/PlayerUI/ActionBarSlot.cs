using UnityEngine;
using System;

public class ActionBarSlot : DragAndDropSlot{
    private KeyCode assignedKey;
    public event Action<SlotData<KeyCode>> OnSlotUpdated;

    public override void SetItem(GameObject item, bool f = false)
    {
        base.SetItem(item);

        if(!f) return;
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>()?.GetItem;
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, itemData));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();
        if(!f) return;
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, null));
    }
    
    public override bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        if(itemData == null) return false;
        return itemData is Consumable consumable;
    }
}