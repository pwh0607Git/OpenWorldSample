using UnityEngine;
using System;
using TMPro;

public class EquipmentSlot : DragAndDropSlot
{
    public EquipmentType type{get; private set;}
    private Action<SlotData<EquipmentType>> OnSlotUpdated;
    
    public void InitSlotDate(EquipmentType type, Action<SlotData<EquipmentType>> action){
        this.type = type;
        GetComponentInChildren<TextMeshProUGUI>().text = type.ToString();

        OnSlotUpdated += action;
    }

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