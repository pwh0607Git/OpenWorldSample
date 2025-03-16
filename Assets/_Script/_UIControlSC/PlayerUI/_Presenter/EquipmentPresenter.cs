using System.Collections.Generic;
using UnityEngine;

public class EquipmentPresenter
{
    public PlayerStateModel playerStateModel;
    public EquipmentModel equipmentModel;
    public EquipmentView view;

    public EquipmentPresenter(EquipmentModel equipmentModel, PlayerStateModel playerStateModel, EquipmentView view){
        this.equipmentModel = equipmentModel;
        this.playerStateModel = playerStateModel;
        this.view = view;
        
        view.OnViewUpdated += UpdateModelDataFromView;
        equipmentModel.OnModelUpdated += UpdateViewFromModel;
    }

    public void UpdateViewFromModel(){
        Debug.Log($"Equipment Presenter : Update View");
        view.UpdateView(GetList());
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> slot){
        Debug.Log($"Equipment Presenter : Update Model {slot.slotKey} : {slot.itemData}");
        equipmentModel.UpdateModelDataFromView(slot);
    } 
    
    public Dictionary<EquipmentType, Item> GetList() => equipmentModel.GetEquipmentItems();
}