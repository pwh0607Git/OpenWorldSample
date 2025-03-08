using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryModel 
{
    private Dictionary<int, Item> items = new Dictionary<int, Item>();
    public event Action OnModelUpdated; 

    public Dictionary<int, Item> GetItemList() => new Dictionary<int, Item>(items);
    public InventoryModel(int maxSlotSize){
        for(int idx = 0; idx < maxSlotSize; idx++){
            items[idx] = null;
        }
        OnModelUpdated?.Invoke();          
    }

    public void InitModel(List<SlotData<int>> slotDataList){
        Debug.Log($"Inventory Model : Init => {slotDataList.Count}");
        foreach(SlotData<int> data in slotDataList){
            items[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
        }
        // OnModelUpdated?.Invoke();
    }

    // 먹은 아이템.
    public bool AddItem(ItemData itemData)
    {
        Item existingItem = FindExistingItem(itemData);
        
        if (existingItem != null && existingItem is Consumable consumable)
        {
            consumable.GetThisItem();
            OnModelUpdated?.Invoke();
            return true;
        }

        return AddNewItem(itemData);
    }

    private bool AddNewItem(ItemData data)
    {
        int index = SearchEmptyIndex();
        if (index == -1) return false;

        items[index] = ItemFactory.CreateItem(data);
        OnModelUpdated?.Invoke();
        return true;
    }


    public int SearchEmptyIndex(){
        int index = -1;
        foreach(var item in items){
            if(item.Value == null) return item.Key;
        }
        return index;
    }

    private Item FindExistingItem(ItemData newItem)
    {
        foreach (var item in items.Values)
        {
            // 올바르지 못한 아이템이거나, 둘이 다른 아이템이면 넘어가기
            if (item == null) continue;

            if (newItem is ConsumableData consumable1 && item.data is ConsumableData consumable2)
            {
                if (consumable1.subType == consumable2.subType)
                {
                    return item;
                }
            }
            else if (newItem is EquipmentData equipment1 && item.data is EquipmentData equipment2)
            {
                if (equipment1.subType == equipment2.subType)
                {
                    return item;
                }
            }
        }
        return null;
    }

    // 뷰로부터 같은 데이터 이므로 뷰를 갱신하는 호출은 수행하지 않는다.
    public void UpdateModelDataFromView(SlotData<int> data)
    {
        Debug.Log($"Inventory Model : 슬롯 {data.slotKey} 업데이트");
        items[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }
}