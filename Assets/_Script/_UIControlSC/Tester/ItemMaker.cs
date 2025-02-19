using System;
using System.Collections;
using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [Header("Consumable")]
    public ItemData hp;
    public ItemData mp;

    [Header("Equipment")]
    public ItemData equipment1;

    [SerializeField] List<ItemEntry> _serList;
    [SerializeField] List<ItemData> _newItemList;

    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(NotifyInitTest());   
    }
    IEnumerator NotifyInitTest(){
        yield return new WaitForSeconds(0.5f);
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
public class ItemEntry{
    [SerializeField] public int inventoryIdx;
    [SerializeField] public ItemData indexItem;
    [SerializeField] public int count;

    public ItemEntry(int idx, ItemData itemData, int count){
        this.inventoryIdx = idx;
        this.indexItem =itemData;
        this.count = count;
    }
}