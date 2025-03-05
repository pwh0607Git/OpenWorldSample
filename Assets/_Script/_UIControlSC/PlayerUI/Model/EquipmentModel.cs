using System.Collections.Generic;
using UnityEngine;
using System;

//장비 슬롯은 고정위치.
public class EquipmentModel : MonoBehaviour
{
    private Dictionary<EquipmentType, ItemData> equipedItems = new Dictionary<EquipmentType, ItemData>();

    public event Action OnModelUpdated;
    public EquipmentModel(){

    }
    public void InitModel(List<SlotData<EquipmentType>> slotDatas){
        // UpdateModel(components);
        foreach(var data in slotDatas){
            UpdateModel(data);
        }
        OnModelUpdated?.Invoke();
    }

    public void UpdateModel(SlotData<EquipmentType> data){
        equipedItems[data.slotKey] = data.item;
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> data)
    {
        Debug.Log($"Inventory Model : 슬롯 {data.slotKey} 업데이트");
        equipedItems[data.slotKey] = data.item;
    }   

    public Dictionary<EquipmentType, ItemData> GetEquipmentItems() =>  new Dictionary<EquipmentType, ItemData>(equipedItems);
}