using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryModel 
{
    private Dictionary<int, ItemData> items = new Dictionary<int, ItemData>();
    public event Action OnModelUpdated; 

    public InventoryModel(int maxSlotSize){
        for(int idx = 0; idx < maxSlotSize; idx++){
            items[idx] = null;
        }
        OnModelUpdated?.Invoke();           // 0-0-0
    }
    public void InitModel(List<SlotData<int>> itemList){
        Debug.Log($"Inventory Model : Init => {itemList.Count}");
        foreach(SlotData<int> item in itemList){
            items[item.slotKey] = item.item; 
        }
        
    }

    public bool AddItem(ItemData item){
        Debug.Log($"Model : AddItem - {item}");
        bool res = HandleGetItemData(item);
        OnModelUpdated?.Invoke();
        return res;
    }

    public Dictionary<int, ItemData> GetItemList() => new Dictionary<int, ItemData>(items);

    public int SearchEmptyIndex(){
        int index = -1;
        foreach(var item in items){
            if(item.Value == null) return item.Key;
        }
        return index;
    }

    // 아이템을 얻었을 때.
    public bool HandleGetItemData(ItemData getItem){
        Debug.Log($"{getItem} 데이터 처리...");
        // 먼저 해당 아이템이 어떤 아이템인지 확인하기
        if(getItem is Consumable consumable){
            //가지고 있는 아이템인지 확인하기
            bool isExsiting = SearchItemByType<ConsumableType>(getItem.itemType, consumable.subType);
            if(isExsiting){
                consumable.GetThisItem();
                return true;
            }else{
                return GetNewItem(getItem);
            }
        }else if(getItem is Equipment equipment){
            return GetNewItem(getItem);         //그냥 새로운 슬롯에 할당하기.
        }
        return false;
    }

    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        foreach (var targetItem in items)
        {
            if (targetItem.Value == null) continue;

            ItemData item = targetItem.Value;

            if (item.itemType == itemType)
            {
                if (subType == null)
                {
                    Debug.Log("Search Code : 001");
                    return true;
                }

                if (itemType == ItemType.Consumable && item is Consumable consumable)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)consumable.subType, subType.Value))
                    {
                        Debug.Log($"Search Code : 002 - Found matching item: {consumable.subType}");
                        return true;
                    }
                }
                else if (itemType == ItemType.Equipment && item is Equipment equipment)
                {
                    if (EqualityComparer<T>.Default.Equals((T)(object)equipment.subType, subType.Value))
                    {
                        Debug.Log("Search Code : 003");
                        return true;
                    }
                }
            }
        }
        Debug.Log("Search Code : 004");
        return false;
    }

    bool GetNewItem(ItemData item){
        int index = SearchEmptyIndex();
        if(index == -1) return false;           
        items[index] = item;
        return true;
    }

    // 뷰로부터 같은 데이터 이므로 뷰를 갱신하는 호출은 수행하지 않는다.
    public void UpdateModelDataFromView(SlotData<int> item)
    {
        Debug.Log($"Inventory Model : 슬롯 {item.slotKey} 업데이트");
        items[item.slotKey] = item.item;
    }
}