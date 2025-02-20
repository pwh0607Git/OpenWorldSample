using UnityEngine;
using System.Collections.Generic;

public class InventoryPresenter
{
    private InventoryModel model;
    private InventoryView view;
    public bool isCompletedUpdateView;

    public InventoryPresenter(InventoryModel model, InventoryView view){
        this.model = model;
        this.view = view;
        model.OnModelChanged += ModelChangeHandler;
        view.OnChangedInventoryView += ViewChangeHandler;
        isCompletedUpdateView = false;
        view.CreateSlots(model.maxSlotSize);
    }

    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateView(model.GetItemList());
        }
    }

    //View에서 변화가 일어났을 때...
    public void ViewChangeHandler(List<ItemEntry> entries){
        foreach(ItemEntry entry in entries){
            Debug.Log($"{entry.inventoryIdx} : {entry.indexItem}");
        }
        Debug.Log("View가 변경되었다! Model을 Update하러 가자!");
        UpdateModel(entries);
    }
    
    //Model에서의 데이터 변화가 일어 났을 때...
    public void ModelChangeHandler(){
        Debug.Log("Model이 변경되었다! View를 Update하러 가자!");
        UpdateView();
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
        model.UpdateModel(entries);         // 이벤트 처리로 수정 예정 
    }

    public void SerializeModel(List<ItemEntry> entries){
        Debug.Log("Inventory Presenter : Serialize");
        model.SerializeModel(entries);
    }

    //icon이동으로는 UpdateView 발생 X 모델의 변화만 발생하도록
    public void UpdateView(){
        Debug.Log($"Presenter : model itemList Count : {model.GetItemList().Count}");
        view.UpdateView(model.GetItemList());
    }
}
