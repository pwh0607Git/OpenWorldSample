using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

public class ModelViewer : MonoBehaviour
{
    public PlayerUIPresenter playerUIPresenter;

    [SerializeField] List<SlotData<int>> InventoryList;
    [Button("ShowInventoryModel"), HideField] public bool btn2;
    [Space(20)]
    [SerializeField] List<SlotData<KeyCode>> ActionBarList;
 
    [Button("ShowActionbarModel"), HideField] public bool btn1;
 
    void ShowInventoryModel(){
        Dictionary<int, Item> dic = playerUIPresenter.GetInventoryModel();
        InventoryList.Clear();

        foreach(var item in dic){
            if(item.Value == null) continue;
            InventoryList.Add(new SlotData<int>(item.Key, item.Value.data));
        }
    }
    
    void ShowActionbarModel(){
        Dictionary<KeyCode, Item> dic = playerUIPresenter.GetActionbarModel();
        ActionBarList.Clear();

        foreach(var data in dic){
            ActionBarList.Add(new SlotData<KeyCode>(data.Key, data.Value.data));
        }
    }
}