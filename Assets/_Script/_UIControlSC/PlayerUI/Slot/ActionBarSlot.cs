using UnityEngine;
using System;

public class ActionBarSlot : DragAndDropSlot{
    public KeyCode assignedKey;
    public event Action<SlotData<KeyCode>> OnSlotUpdated;
    public override bool CheckVaildItem(GameObject itemIcon){
        Item item = itemIcon.GetComponentInChildren<ItemIcon>().item;
        if(item == null) return false;
        return item is Consumable;
    }

    void Update()
    {
        if(Input.GetKeyDown(assignedKey)){
            if(assignedItem == null) return;

            if(assignedItem.GetComponent<ItemIcon>().item is Consumable consumable) consumable.Use();
        }
    }

    #region UIITemEventHandler R 
    public override void SetItem(GameObject itemIcon, bool f = false)
    {
        base.SetItem(itemIcon);
        
        if(!f) return;
        ItemIcon icon = itemIcon.GetComponent<ItemIcon>();
        icon.OnItemDestroyed += ClearSlot;
        Item item = icon.item;

        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, item, item.count));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();

        if(!f) return;
        OnSlotUpdated?.Invoke(new SlotData<KeyCode>(assignedKey, null));
    }
    #endregion
}