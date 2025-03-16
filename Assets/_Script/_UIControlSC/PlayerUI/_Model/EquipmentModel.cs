using System.Collections.Generic;
using UnityEngine;
using System;

//장비 슬롯은 고정위치.
public class EquipmentModel : MonoBehaviour
{
    private Dictionary<EquipmentType, Item> equipedItems = new Dictionary<EquipmentType, Item>();

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
        equipedItems[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> data)
    {
        Debug.Log($"Inventory Model : 슬롯 {data.slotKey} 업데이트");
        equipedItems[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }   

    public Dictionary<EquipmentType, Item> GetEquipmentItems() =>  new Dictionary<EquipmentType, Item>(equipedItems);
}