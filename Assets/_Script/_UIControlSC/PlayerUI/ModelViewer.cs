using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

public class ModelViewer : MonoBehaviour
{
    public PlayerUIPresenter playerUIPresenter;

    [SerializeField] List<ItemEntry> InventoryList;
    [Button("ShowInventoryModel"), HideField] public bool btn2;
    [Space(20)]
    [SerializeField] List<ActionBarSlotComponent> ActionBarList;
 
    [Button("ShowActionbarModel"), HideField] public bool btn1;
 
    void ShowInventoryModel(){
        InventoryList = playerUIPresenter.ShowInventoryModel();
    }
    
    void ShowActionbarModel(){
        ActionBarList = playerUIPresenter.ShowActionbarModel();
    }
}