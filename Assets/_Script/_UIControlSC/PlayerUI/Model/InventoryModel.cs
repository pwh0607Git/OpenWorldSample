using System;
using System.Collections.Generic;
public class InventoryModel
{
    private List<ItemData> items = new List<ItemData>();
    public int maxSlotSize {get; private set;}

    public event Action<List<ItemData>> OnInventoryUpdated;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public InventoryModel(int maxSlotSize){
        this.maxSlotSize = maxSlotSize;
    }

    public bool CheckSlotSize(){
        return items.Count < maxSlotSize;
    }
    
    public bool AddItem(ItemData item){
        if(items.Count >= maxSlotSize) return false;
        items.Add(item);
        return true;
    }

    public List<ItemData> GetItemList(){
        return new List<ItemData>(items);               //복사본을 전달한다!
    }
}