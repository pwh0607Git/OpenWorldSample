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

    [SerializeField] List<SlotData<int>> _serList;
    [SerializeField] List<ItemData> _newItemList;

    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(NotifyInitTest());   
    }
    IEnumerator NotifyInitTest(){
        yield return null;
        uiPresenter.InitInventory(_serList);
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
    public ItemData itemData;
    public int count;
    public SlotData(T slotKey, ItemData itemData = null, int count = 1)
    {
        this.slotKey = slotKey;
        this.itemData = itemData;
        this.count = count;
    }
}