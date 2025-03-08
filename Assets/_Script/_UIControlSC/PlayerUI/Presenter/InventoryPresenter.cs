using UnityEngine;
using System.Collections.Generic;

public class InventoryPresenter
{
    private InventoryModel model;
    private InventoryView view;

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        view.OnViewUpdated += UpdateModelDataFromView;
        model.OnModelUpdated += UpdateViewFromModel;
        view.InitSlots(40);
    }

    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateView(model.GetItemList());
        }
    }

    public void AddItem(ItemData itemData)
    {
        model.AddItem(itemData);
    }
    

    public void InitModel(List<SlotData<int>> datas)
    {
        Debug.Log("Inventory Presenter : Init");

        model.InitModel(datas);
        view.UpdateView(model.GetItemList());
        view.EnableSlotEvents();
    }

    public void UpdateViewFromModel(){
        Debug.Log($"Presenter : model itemList Count : {model.GetItemList().Count}");
        view.UpdateView(model.GetItemList());
    }

    public void UpdateModelDataFromView(SlotData<int> slot){
        model.UpdateModelDataFromView(slot);
    } 

    #region Inspector Caller
    public Dictionary<int, Item> GetList() => model.GetItemList();
    #endregion
}