using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePresenter : MonoBehaviour
{
    private PlayerStateModel model;
    private PlayerStateView stateView;
    private PlayerHealthbarView healthbarView;
    private PlayerState currentPlayerState;

    public PlayerStatePresenter(PlayerStateModel model, PlayerStateView stateView, PlayerHealthbarView healthbarView){
        this.model = model;
        this.stateView = stateView;
        this.healthbarView = healthbarView;
        
        model.OnModelUpdated += ModelChangeHandler;
    }

    public void ModelChangeHandler(){
        Debug.Log($"{GetType()} : Model이 변경되었다! View를 Update하러 가자!");

        if(stateView.gameObject.activeSelf){
            stateView.UpdatePlayerStateView(model.GetState());
        }else{
            currentPlayerState = model.GetState();
        }
        
        healthbarView.UpdateHealthbar(model.GetState());
    }

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        Debug.Log($"PlayerDataPresenter : SerializeModel");
        foreach(var data in datas){
            Equipment item = data.item as Equipment;
            EquipItem(item);
        }
    }

    public void ApplyEffect(IStateEffect effect){
        model.ApplyEffect(effect);
    }

    public void ApplyEquipment(Equipment pre, Equipment cur){
        Debug.Log("장비 장착 처리하기@");
        if(pre != null) UnEquipItem(pre);
        if(cur != null) EquipItem(cur);
    }

    public void EquipItem(Equipment equipment){
        Debug.Log($"[{equipment}] 장착 !!");
        model.EquipItem(equipment);
    }

    public void UnEquipItem(Equipment equipment){
        model.UnEquipItem(equipment);
    }
}