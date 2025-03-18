using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EquipmentPresenter
{
    public EquipmentModel model;
    public EquipmentView view;

    public EquipmentPresenter(EquipmentModel model, PlayerStateModel playerStateModel, EquipmentView view){
        this.model = model;
        this.view = view;
        
        view.OnViewUpdated += UpdateModelDataFromView;
        model.OnModelUpdated += UpdateViewFromModel;
    }

    public void UpdateViewFromModel(){
        Debug.Log($"Equipment Presenter : Update View");
        view.UpdateView(GetList());
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> slot){
        Debug.Log($"Equipment Presenter : Update Model {slot.slotKey} : {slot.itemData}");
        model.UpdateModelDataFromView(slot);
    } 

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        // 모델을 초기화 후 view를 갱신.
        model.SerializeModel(datas);
    }
    
    public Dictionary<EquipmentType, Item> GetList() => model.GetEquipmentItems();
}

// Slot Data<EquipmentType>로 통일!!