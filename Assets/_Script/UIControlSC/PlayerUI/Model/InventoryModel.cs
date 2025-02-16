using System.Collections.Generic;
using UnityEngine;

public class InventoryModel
{
    private List<ItemData> items = new List<ItemData>();
    public int maxSlotSize {get; private set;}

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