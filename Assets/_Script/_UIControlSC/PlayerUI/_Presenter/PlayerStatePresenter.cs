using System.Collections.Generic;
using UnityEngine;

public class PlayerStatePresenter : MonoBehaviour
{
    private PlayerStateModel model;
    private PlayerStateView stateView;
    private PlayerHealthbarView healthbarView;
    private BuffStateView buffStateView;
    private PlayerState currentPlayerState;

    public PlayerStatePresenter(PlayerStateModel model, PlayerStateView stateView, PlayerHealthbarView healthbarView, BuffStateView buffStateView){
        this.model = model;
        this.stateView = stateView;
        this.healthbarView = healthbarView;
        this.buffStateView = buffStateView;

        model.OnModelUpdated += ModelChangeHandler;
        buffStateView.OnBuffStart += BuffStartHandler;
        buffStateView.OnBuffEnd += BuffEndHandler;
    }

    public void ModelChangeHandler(){
        if(stateView.gameObject.activeSelf) stateView.UpdatePlayerStateView(model.GetState());
        else currentPlayerState = model.GetState();
        
        healthbarView.UpdateHealthbar(model.GetState());
    }

    public PlayerState GetPlayerState() => model.GetState();

    public void UpdateBuff(IStateEffect effect){
        healthbarView.UpdateBuffPart(effect);
    }

    public void SerializeModel(List<SlotData<EquipmentType>> datas){
        foreach(var data in datas){
            Equipment item = data.item as Equipment;
            EquipItem(item);
        }
    }

    public void ApplyEffect(IStateEffect effect){
        if(effect.GetData().duration <= 0) model.ApplyEffect(effect);
        else buffStateView.OnBuff(effect);
    }

    void BuffStartHandler(IStateEffect effect){
        model.ApplyEffect(effect);
    }

    void BuffEndHandler(IStateEffect effect){
        model.RemoveEffect(effect);
    }

    public void ApplyEquipment(Equipment pre, Equipment cur){
        if(pre != null) UnEquipItem(pre);
        if(cur != null) EquipItem(cur);
    }

    public void EquipItem(Equipment equipment){
        model.EquipItem(equipment);
    }

    public void UnEquipItem(Equipment equipment){
        model.UnEquipItem(equipment);
    }
}