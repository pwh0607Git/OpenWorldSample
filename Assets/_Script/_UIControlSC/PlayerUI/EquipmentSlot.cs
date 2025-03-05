using UnityEngine;
using System;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType type;
    
    public event Action<SlotData<EquipmentType>> OnSlotUpdated;
    
    public override void SetItem(GameObject item, bool f = false)
    {
        base.SetItem(item);
        
        if(!f) return;
        Debug.Log("Slot Set Item...");
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>()?.GetItem;
        OnSlotUpdated?.Invoke(new SlotData<EquipmentType>(type, itemData));
    }

    public override void ClearSlot(bool f = false)
    {
        base.ClearSlot();

        if(!f) return;
        Debug.Log("Slot Clear Item...");
        OnSlotUpdated?.Invoke(new SlotData<EquipmentType>(type, null));
    }
    
    public override bool CheckVaildItem(GameObject item){
        ItemData itemData = item.GetComponentInChildren<ItemDataHandler>().GetItem;
        if(itemData == null) return false;

        if(itemData is Equipment equipment){
            if(equipment.subType != type) return false;
        }
        return true;
    }
}