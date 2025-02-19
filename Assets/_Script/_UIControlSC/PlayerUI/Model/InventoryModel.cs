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
        if(itemDictionary.Count >= maxSlotSize) return false;
        itemDictionary.Add(0,item);            //test용
        return true;
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

    //아이템의 존재여부.
    public bool IsExistingItem(ItemData item){
        for(int i=0;i<maxSlotSize;i++){
            if(itemDictionary[i] == null) continue;
            
            if(itemDictionary[i].Equals(item)) return true;
        }
        return false;
    }

    //List<ItemEntry>의 경우에는 외부 DB로 부터의 데이터를 동기화할때만 사용한다.
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
    }
}