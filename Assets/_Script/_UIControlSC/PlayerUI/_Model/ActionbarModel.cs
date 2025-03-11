using System;
using System.Collections.Generic;
using UnityEngine;

public class ActionbarModel 
{
    public int maxSlotSize {get; private set;}
    private Dictionary<KeyCode, Item> slotDatas = new Dictionary<KeyCode, Item>();
    public Dictionary<KeyCode, Item> GetSlotDatas() => new(slotDatas);
    public event Action OnModelUpdated;             // inventory 내 아이템 정보가 갱신되면 실행되는 이벤트.

    public ActionbarModel()
    {
        this.maxSlotSize = maxSlotSize;
    }

    public void InitModel(Dictionary<KeyCode, Item> slotDatas){
        Debug.Log($"Actionbar Model : Init => {slotDatas.Count}");
        this.slotDatas = slotDatas;
        OnModelUpdated?.Invoke();
    }

    public void UpdateModel(SlotData<KeyCode> data){
        Debug.Log("Model Update...");
        slotDatas[data.slotKey] = ItemFactory.CreateItem(data.itemData, data.count);
    }
}