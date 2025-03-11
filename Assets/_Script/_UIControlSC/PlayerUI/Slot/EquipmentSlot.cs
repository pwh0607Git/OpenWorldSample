using UnityEngine;
using System;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType type;
    
    public event Action<SlotData<EquipmentType>> OnSlotUpdated;
    
    public override void SetItem(GameObject itemIcon, bool f = false)
    {
        base.SetItem(itemIcon);
        
        if(!f) return;
        Debug.Log("Slot Set Item...");
        Item item = itemIcon.GetComponent<ItemIcon>().item;
        OnSlotUpdated?.Invoke(new SlotData<EquipmentType>(type, item.data));
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