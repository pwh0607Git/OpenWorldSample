using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

public class ModelViewer : MonoBehaviour
{
    public PlayerUIPresenter playerUIPresenter;

    [SerializeField] List<SlotData<int>> InventoryList;
    [Button("ShowInventoryModel"), HideField] public bool btn2;
    [Space(20)]
    [SerializeField] List<ActionBarSlotComponent> ActionBarList;
 
    [Button("ShowActionbarModel"), HideField] public bool btn1;
 
    void ShowInventoryModel(){
        Dictionary<int, ItemData> dic = playerUIPresenter.GetInventoryModel();
        Debug.Log($"Show! Inventory {dic.Count}");
        InventoryList.Clear();

        foreach(var item in dic){
            if(item.Value == null) continue;
            InventoryList.Add(new SlotData<int>(item.Key, item.Value));
        }
    }
    
    void ShowActionbarModel(){
        ActionBarList = playerUIPresenter.ShowActionbarModel();
    }
}