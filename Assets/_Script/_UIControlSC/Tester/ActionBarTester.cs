using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBarTester : MonoBehaviour
{
    public List<SerializeItemData<KeyCode>> components;
    public PlayerUIPresenter uiPresenter;

    void Start()
    {
        StartCoroutine(NotifyInitTest());
    }
    IEnumerator NotifyInitTest(){
        yield return null;
        List<SlotData<KeyCode>> itemList = new();
        foreach(var data in components){
            Item item = ItemFactory.CreateItem(data.item, data.count);
            itemList.Add(new SlotData<KeyCode>(data.slotKey, item, data.count));
        }
        uiPresenter.InitActionbar(itemList);
    }
}