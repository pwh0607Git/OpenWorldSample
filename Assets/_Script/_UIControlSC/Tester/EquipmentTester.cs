using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EquipmentTester : MonoBehaviour
{
    
    [Header("Reference")]
    public PlayerUIPresenter uiPresenter;

    [SerializeField] List<SerializeItemData<EquipmentType>> datas;

    void Start()
    {
        SendEquipmentData();
    }

    IEnumerator SendEquipmentData(){
        yield return null;
        List<SlotData<EquipmentType>> itemList = new();
        foreach(var data in datas){
            Item item = ItemFactory.CreateItem(data.item, data.count);
            itemList.Add(new SlotData<EquipmentType>(data.slotKey, item, data.count));
        }
        uiPresenter.SerializeEquipment(itemList);
    }
}
