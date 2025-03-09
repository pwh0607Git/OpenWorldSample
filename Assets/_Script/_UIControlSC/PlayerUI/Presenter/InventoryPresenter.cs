using UnityEngine;
using System.Collections.Generic;

public class InventoryPresenter
{
    private InventoryModel model;
    private InventoryView view;

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        view.OnViewUpdated += UpdateModel;
        model.OnModelUpdated += UpdateView;
        view.InitSlots(40);
    }
    
    public void InitModel(List<SlotData<int>> datas)
    {
        Debug.Log("Inventory Presenter : Init");
        model.InitModel(datas);
    }
    
    public void UpdateView(){
        Debug.Log($"Presenter : model itemList Count : {model.GetItemList().Count}");
        view.UpdateView(model.GetItemList());
    }

    public void UpdateModel(SlotData<int> slot){
        model.UpdateModel(slot);
    } 

    public void AddItem(ItemData itemData)
    {
        model.AddItem(itemData);
    }
    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateView(model.GetItemList());
        }
    }

    public Item GetItemInstance(ItemData itemData){
        Item item = model.FindExistingItem(itemData);
        if(item == null) return null;
        return item;
    }
    #region Inspector Caller
    public Dictionary<int, Item> GetList() => model.GetItemList();
    #endregion
}