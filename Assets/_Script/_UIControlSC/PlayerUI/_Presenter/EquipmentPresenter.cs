using System.Collections.Generic;
using System;

public class EquipmentPresenter
{
    public EquipmentModel model;
    public EquipmentView view;

    public event Action<Equipment> OnItemEquiped;
    public event Action<Equipment> OnItemUnEquiped;
    public EquipmentPresenter(EquipmentModel model, PlayerStateModel playerStateModel, EquipmentView view){
        this.model = model;
        this.view = view;
        
        view.OnViewUpdated += UpdateModelDataFromView;
    }

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        // 모델을 초기화 후 view를 갱신.
        model.SerializeModel(datas);
        view.SerializeView(datas);
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> slot){
        model.UpdateModelDataFromView(slot);
    } 

    public void EquipItem(SlotData<EquipmentType> data){
        model.UpdateModel(data);
    }

    public void UnEquipItem(SlotData<EquipmentType> data){
        model.UpdateModel(data);
    }

    public Dictionary<EquipmentType, Item> GetList() => model.GetEquipmentItems();
}