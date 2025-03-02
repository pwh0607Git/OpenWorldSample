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
        view.CreateSlots(40);
    }

    public void ToggleInventory(){
        bool isActive = !view.inventoryWindow.activeSelf;
        view.SetActive(isActive);

        if(isActive){
            view.UpdateView(model.GetItemList());
        }
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
    
    //인벤토리 데이터 받기.
    public void UpdateModel(SlotData<int> data){
        Debug.Log("Inventory Presenter : Update");
        model.UpdateModel(data);         // 이벤트 처리로 수정 예정 
    }

    public void InitModel(List<SlotData<int>> datas){
        Debug.Log("Inventory Presenter : Serialize");
        model.InitModel(datas);
    }

    //icon이동으로는 UpdateView 발생 X 모델의 변화만 발생하도록
    public void UpdateView(){
        Debug.Log($"Presenter : model itemList Count : {model.GetItemList().Count}");
        view.UpdateView(model.GetItemList());
    }

    public void UpdateSlot(SlotData<int> slot){
        model.UpdateModel(slot);
    }

    public Dictionary<int, ItemData> GetList(){
        return model.GetItemList();
    }
}
