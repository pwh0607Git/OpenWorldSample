using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [Header("Consumable")]
    public Item hpPotion;
    public Item mpPotion;

    [Header("Equipment")]
    public Item equipment1;

    [SerializeField] List<SerializeItemData<int>> _serList;
    [SerializeField] List<ItemData> _newItemList;

    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(NotifyInitTest());   
    }
    IEnumerator NotifyInitTest(){
        yield return null;
        List<SlotData<int>> itemList = new();
        foreach(var data in _serList){
            Item item = ItemFactory.CreateItem(data.item, data.count);
            itemList.Add(new SlotData<int>(data.slotKey, item, data.count));
        }
        uiPresenter.InitInventory(itemList);
    }

    [Button("GetItem"), HideField] public bool btn1;
    public void GetItem(){
        Debug.Log("Get Item");

        //랜덤으로 아이템 부여
        int rnd = UnityEngine.Random.Range(0, _newItemList.Count);
        uiPresenter.GetItem(_newItemList[rnd]);
    }
}

[Serializable]
public class SlotData<T>
{
    public T slotKey;   // InventorySlot: int, ActionBarSlot: KeyCode
    public Item item;
    public int count;
    public SlotData(T slotKey, Item item = null, int count = 1)
    {
        this.slotKey = slotKey;
        this.item = item;
        this.count = count;
    }
}

[Serializable]
public class SerializeItemData<T>{
    public T slotKey;   // InventorySlot: int, ActionBarSlot: KeyCode
    public ItemData item;
    public int count;
    public SerializeItemData(T slotKey, ItemData item = null, int count = 1)
    {
        this.slotKey = slotKey;
        this.item = item;
        this.count = count;
    }
}