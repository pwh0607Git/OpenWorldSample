using System.Collections.Generic;
using UnityEngine;
using System;

public class EquipmentModel : MonoBehaviour
{
    private Dictionary<EquipmentType, Item> equipedItems = new Dictionary<EquipmentType, Item>();

    public event Action<Equipment, Equipment> OnModelUpdated;
    public EquipmentModel(){ }

    public void SerializeModel(List<SlotData<EquipmentType>> slotDatas){
        foreach(var data in slotDatas){
            UpdateModel(data);
        }
    }

    public void UpdateModel(SlotData<EquipmentType> data){
        equipedItems[data.slotKey] = data.item;
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> data)
    {
        Debug.Log($"Equipment Model : 슬롯 {data.slotKey} 업데이트");
        Equipment prev = equipedItems[data.slotKey] as Equipment;
        equipedItems[data.slotKey] = data.item;
        OnModelUpdated?.Invoke(prev, equipedItems[data.slotKey] as Equipment);
    }   

    public Dictionary<EquipmentType, Item> GetEquipmentItems() =>  new Dictionary<EquipmentType, Item>(equipedItems);
}