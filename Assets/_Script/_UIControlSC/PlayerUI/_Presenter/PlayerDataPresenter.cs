using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPresenter
{
    private PlayerStateModel model;
    private PlayerStateView stateView;
    private PlayerHealthbarView healthbarView;
    private PlayerState currentPlayerState;

    public PlayerDataPresenter(PlayerStateModel model, PlayerStateView stateView, PlayerHealthbarView healthbarView){
        this.model = model;
        this.stateView = stateView;
        this.healthbarView = healthbarView;

        model.OnModelUpdated += ModelChangeHandler;
    }

    public void ModelChangeHandler(){
        Debug.Log($"{GetType()} : Mode  l이 변경되었다! View를 Update하러 가자!");

        if(stateView.gameObject.activeSelf){
            stateView.UpdatePlayerStateView(model.GetState());
        }else{
            currentPlayerState = model.GetState();
        }
        
        healthbarView.UpdateView(model.GetState());
    }

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        Debug.Log($"PlayerDataPresenter : SerializeModel");
        foreach(var equipment in datas){
            Equipment item = ItemFactory.CreateItem(equipment.itemData) as Equipment;
            EquipItem(item);
        }
    }

    public void ApplyEffect(IStateEffect effect){
        model.ApplyEffect(effect);
    }

    public void EquipItem(Equipment equipment){
        Debug.Log($"[{equipment}] 장착 !!");
        model.EquipItem(equipment);
    }

    public void UnequipItem(Equipment equipment){    
        Debug.Log($"[{equipment}] 해제 !!");
        model.EquipItem(equipment);
    }

    public void TogglePlayerDataView(){
        WindowController window = stateView.GetComponentInParent<WindowController>();

        bool isActive = !window.gameObject.activeSelf;
        window.gameObject.SetActive(isActive);
        
        if(isActive) stateView.UpdatePlayerStateView(currentPlayerState);
    }
}