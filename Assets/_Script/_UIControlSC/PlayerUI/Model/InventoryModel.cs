using System;
using UnityEngine;
using System.Collections.Generic;

public class InventoryModel 
{
    private Dictionary<int, ItemData> itemDictionary = new Dictionary<int, ItemData>();
    public int maxSlotSize {get; private set;}

    public event Action OnModelChanged;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public InventoryModel(int maxSlotSize){
        this.maxSlotSize = maxSlotSize;
        for(int i=0;i<maxSlotSize;i++){
            itemDictionary.Add(i, null);
        }
    }

    public bool CheckSlotSize(){
        return itemDictionary.Count < maxSlotSize;
    }
    
    // 조건 정립
    // 1. 이미 존재하는 아이템인가?
    // 2. 새로운 아이템인가?
    // 2-1. 빈 인덱스를 찾아 해당 인덱스
    public bool AddItem(ItemData item){
        // 이미 가지고 있는 아이템인지 확인하기
        // 이미 가지고 있으면 Count만 증가
        // 새롭거나 장비 아이템이면 빈슬롯에 데이터 추가
        Debug.Log($"Model : {item}");
        bool res = HandleGetItemData(item);
        
        return res;
    }

    public Dictionary<int, ItemData> GetItemList(){
        return new Dictionary<int, ItemData>(itemDictionary);               //복사본을 전달한다!
    }

    public int SearchEmptyIndex(){
        int index = -1;
        for(int i=0;i<maxSlotSize;i++){
            if(itemDictionary[i] == null) return i;
        }
        return index;
    }

    public bool HandleGetItemData(ItemData item){
        for(int i=0;i<maxSlotSize;i++){
            if(itemDictionary[i] == null) continue;
            ItemData itemInInventory = itemDictionary[i];
        
            if (itemInInventory != null)
            {
                if (itemDictionary[i] is Consumable consumable)
                {
                    if (SearchItemByType<ConsumableType>(itemInInventory.itemType, consumable.subType))
                    {
                        consumable.GetThisItem();
                    }
                    else
                    {
                        if(GetNewItem(itemInInventory)) return true;
                    }
                }
                else if (itemInInventory is Equipment equipment)
                {
                    if(GetNewItem(itemInInventory)) return true;
                }
            }
            else
            {
                Debug.Log("ItemData is Null...");
            }
        }
        return false;
    }

    //해당 아이템을 이미 소유하고 있는지.
    public bool SearchItemByType<T>(ItemType itemType, T? subType = null) where T : struct
    {
        foreach (var targetItem in itemDictionary)
        {
            if (targetItem.Value == null) continue;

            ItemData item = targetItem.Value;

            if (item.itemType == itemType)
            {
                if (subType == null)
                {
                    Debug.Log("Search Code : 001");
                    return true;           //��Ÿ ������
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

    /*
        1. 해당아이템은 가지고 있던 것인가?
        2. 새로운 아이템 인가?
        2-1. 빈 슬롯이 있는가?
    */
    bool GetNewItem(ItemData item){
        int index = SearchEmptyIndex();
        if(index == -1) return false;            //빈 공간이 없다는 뜻
        itemDictionary.Add(index,item);            //test용
        return true;
    }

    //List<ItemEntry>의 경우에는 외부 DB로 부터의 데이터를 동기화할때만 사용한다. => Serialized로 변경 예정
    public void UpdateModel(List<ItemEntry> items){
        ClearDictionary();
        foreach(var entry in items){
            Debug.Log("Inventory 모델 : Update");
            itemDictionary[entry.inventoryIdx] = entry.indexItem;
        }
        OnModelChanged?.Invoke();
    }

    void ClearDictionary(){
        for(int i=0;i<maxSlotSize;i++){
            itemDictionary[i] = null;
        }
        //itemDictionary.Clear()
    }
}