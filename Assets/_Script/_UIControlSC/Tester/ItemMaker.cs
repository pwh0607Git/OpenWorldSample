using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMaker : MonoBehaviour
{
    [Header("Consumable")]
    public ItemData hp;
    public ItemData mp;

    [Header("Equipment")]
    public ItemData equipment1;

    [SerializeField] List<ItemEntry> _serList;

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