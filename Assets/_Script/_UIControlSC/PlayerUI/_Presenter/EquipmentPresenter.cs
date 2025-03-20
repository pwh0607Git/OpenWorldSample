using System;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPresenter
{
    public EquipmentModel model;
    public EquipmentView view;
    public event Action<Equipment, Equipment> OnEquipmentChanged;

    public EquipmentPresenter(EquipmentModel model, PlayerStateModel playerStateModel, EquipmentView view){
        this.model = model;
        this.view = view;
        
        view.OnViewUpdated += UpdateModelDataFromView;
        model.OnModelUpdated += TriggerEquipmentChanged;
    }

    private void TriggerEquipmentChanged(Equipment pre, Equipment cur){
        OnEquipmentChanged?.Invoke(pre, cur);
    }

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        // 모델을 초기화 후 view를 갱신.
        Debug.Log($"EquipmentPresenter : {datas.Count}");
        model.SerializeModel(datas);
        view.SerializeView(datas);
    }

    public void UpdateModelDataFromView(SlotData<EquipmentType> slot){
        model.UpdateModelDataFromView(slot);
    }

    public Dictionary<EquipmentType, Item> GetList() => model.GetEquipmentItems();
}