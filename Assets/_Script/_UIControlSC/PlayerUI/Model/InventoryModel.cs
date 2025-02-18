using System;
using System.Collections.Generic;
public class InventoryModel
{
    // private List<ItemEntry> items = new List<ItemEntry>();
    //index[key] : ItemData[value]
    private Dictionary<int, ItemData> itemsDictionary = new Dictionary<int, ItemData>();
    public int maxSlotSize {get; private set;}

    public event Action<List<ItemData>> OnInventoryUpdated;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public InventoryModel(int maxSlotSize){
        this.maxSlotSize = maxSlotSize;
        for(int i=0;i<maxSlotSize;i++){
            itemsDictionary.Add(i, null);
        }
    }

    public bool CheckSlotSize(){
        return itemsDictionary.Count < maxSlotSize;
    }
    
    // 조건 정립
    // 1. 이미 존재하는 아이템인가?
    // 2. 새로운 아이템인가?
    // 2-1. 빈 인덱스를 찾아 해당 인덱스
    public bool AddItem(ItemData item){
        if(itemsDictionary.Count >= maxSlotSize) return false;
        itemsDictionary.Add(0,item);            //test용
        return true;
    }

    public Dictionary<int, ItemData> GetItemList(){
        return new Dictionary<int, ItemData>(itemsDictionary);               //복사본을 전달한다!
    }

    public int SearchEmptyIndex(){
        int index = -1;
        for(int i=0;i<maxSlotSize;i++){
            if(itemsDictionary[i] == null) return i;
        }
        return index;
    }

    //아이템의 존재여부.
    public bool IsExistingItem(ItemData item){
        for(int i=0;i<maxSlotSize;i++){
            if(itemsDictionary[i] == null) continue;
            
            if(itemsDictionary[i].Equals(item)) return true;
        }
        return false;
    }

    public void UpdateModel(List<ItemEntry> items){
        foreach(var entry in items){
            itemsDictionary.Add(entry.invenIdx, entry.indexItem);
        }
    }
}