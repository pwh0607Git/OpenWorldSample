using UnityEngine;
using System.Collections.Generic;

public class InventoryPresenter
{
    private InventoryModel model;
    private InventoryView view;

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        model.OnModelChanged += ModelChangeHandler;
        view.OnChangedInventoryView += ViewChangeHandler;
        view.CreateSlots(model.maxSlotSize);
    }

    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateView(model.GetItemList());
        }
    }

    public void ViewChangeHandler(List<ItemEntry> entries){
        foreach(ItemEntry entry in entries){
            Debug.Log($"{entry.inventoryIdx} : {entry.indexItem}");
        }
        UpdateModel(entries);
    }

    public void ModelChangeHandler(){
        Debug.Log("Model이 변경되었다! View를 변경하러 가자!");
        view.UpdateView(model.GetItemList());
    }

    public void AddItem(ItemData item)
    {
        if (model.AddItem(item))
        {
            view.UpdateView(model.GetItemList());
        }
    }

    public bool CheckSlotSize()
    {
        return model.CheckSlotSize();
    }

    public void UpdateModel(List<ItemEntry> entries){
        Debug.Log("Inventory Presenter : Update");
        model.UpdateModel(entries);
        UpdateView();
    }
    public void UpdateView(){
        view.UpdateView(model.GetItemList());
    }
}
