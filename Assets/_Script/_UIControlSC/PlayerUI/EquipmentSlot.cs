using UnityEngine;
using System;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType type;
    
    public event Action<SlotData<EquipmentType>> OnSlotUpdated;
    
    public override void SetItem(GameObject ItemIcon, bool f = false)
    {
        base.SetItem(ItemIcon);
        
        if(!f) return;
        Debug.Log("Slot Set Item...");
        Item item = ItemIcon.GetComponentInChildren<Item>();
        OnSlotUpdated?.Invoke(new SlotData<EquipmentType>(type, item.data, item.count));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();

        if(!f) return;
        Debug.Log("Slot Clear Item...");
        OnSlotUpdated?.Invoke(new SlotData<EquipmentType>(type, null));
    }
    
    public override bool CheckVaildItem(GameObject itemIcon){
        Item item = itemIcon.GetComponent<ItemIcon>().item;
        if(item == null) return false;

        if(item.data is EquipmentData data){
            if(data.subType != type) return false;
        }
        return true;
    }
}