using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionbarModel 
{
    public int maxSlotSize {get; private set;}
    private Dictionary<KeyCode, Item> slotDatas = new Dictionary<KeyCode, Item>();
    public event Action OnModelUpdated;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public ActionbarModel()
    {
        this.maxSlotSize = maxSlotSize;
    }
    public void InitModel(List<SlotData<KeyCode>> slotDatas){
        // UpdateModel(components);
        foreach(var data in slotDatas){
            UpdateModel(data);
        }
        OnModelUpdated?.Invoke();
    }
    public Dictionary<KeyCode, Item> GetSlotDatas() => new Dictionary<KeyCode, Item>(slotDatas);

    public void UpdateModel(SlotData<KeyCode> data){
        slotDatas[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }

    public void UpdateModelDataFromView(SlotData<KeyCode> data)
    {
        Debug.Log($"Inventory Model : 슬롯 {data.slotKey} 업데이트");
        slotDatas[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }   
}