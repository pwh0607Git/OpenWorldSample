using System.Collections.Generic;
using UnityEngine;

public class EquipmentPresenter
{
    public PlayerStateModel stateModel;
    public EquipmentModel equipmentModel;
    public EquipmentView view;

    public EquipmentPresenter(PlayerStateModel stateModel, EquipmentModel equipmentModel, EquipmentView view){
        this.stateModel = stateModel;
        this.equipmentModel = equipmentModel;
        this.view = view;
        
        view.OnViewUpdated += UpdateModelDataFromView;
        stateModel.OnModelUpdated += UpdateViewFromModel;
        equipmentModel.OnModelUpdated += UpdateViewFromModel;
    }

    public void UpdateViewFromModel(){
        Debug.Log($"Equipment Presenter : Update View");
        view.UpdateView(GetList());
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> slot){
        Debug.Log($"Equipment Presenter : Update Model {slot.slotKey} : {slot.item}");
        equipmentModel.UpdateModelDataFromView(slot);
    } 
    
    public Dictionary<EquipmentType, ItemData> GetList() => equipmentModel.GetEquipmentItems();
}