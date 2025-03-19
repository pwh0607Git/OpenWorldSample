using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPresenter
{
    private PlayerStateModel stateModel;
    private PlayerStateView stateView;
    private PlayerHealthbarView healthbarView;
    private PlayerState currentPlayerState;

    private PlayerStatePresenter playerStatePresenter;
    private EquipmentPresenter equipmentPresenter;
    
    //변동 방향성 
    // model -> view
    // view -> model

    // PlayerDataPresenter 는 Equipment와 PlayerState를 동기화하는 역할을 수행한다.
    public PlayerDataPresenter(PlayerStatePresenter playerStatePresenter, EquipmentPresenter equipmentPresenter){
        this.playerStatePresenter = playerStatePresenter;
        this.equipmentPresenter = equipmentPresenter;

        equipmentPresenter.OnItemEquiped += ApplyEquipItem;
        equipmentPresenter.OnItemUnEquiped += ApplyUnEquipItem;
    }

    public void SerializePlayerData(List<SlotData<EquipmentType>> datas){
        Debug.Log($"PlayerDataPresenter : SerializeModel");
        playerStatePresenter.SerializeModel(datas);
        equipmentPresenter.SerializeModel(datas);
    }

    //State 갱신
    #region State 갱신
    public void ApplyEffect(IStateEffect effect){
        playerStatePresenter.ApplyEffect(effect);
    }

    public void ApplyEquipItem(Equipment equipment){
        playerStatePresenter.EquipItem(equipment);
    }

    public void ApplyUnEquipItem(Equipment equipment){
        playerStatePresenter.UnEquipItem(equipment);
    }
    #endregion

    public void TogglePlayerDataView(){
        WindowController window = stateView.GetComponentInParent<WindowController>();

        bool isActive = !window.gameObject.activeSelf;
        window.gameObject.SetActive(isActive);
        
        if(isActive) stateView.UpdatePlayerStateView(currentPlayerState);
    }
}