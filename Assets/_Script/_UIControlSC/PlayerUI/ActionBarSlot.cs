using UnityEngine;
using System;

public class ActionBarSlot : DragAndDropSlot{
    private KeyCode assignedKey;
    public event Action<SlotData<KeyCode>> OnSlotUpdated;
    public override bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        if(itemData == null) return false;
        return itemData is Consumable;
    }

    public new void SetItem(GameObject item)
    {
        base.SetItem(item);

        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>()?.GetItem;
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, itemData));
    }

    public new void ClearSlot()
    {
        base.ClearSlot();
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, null));
    }
}