using System.Collections.Generic;
using UnityEngine;

public class PlayerDataPresenter
{
    private PlayerStateView stateView;
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

        equipmentPresenter.OnEquipmentChanged += ApplyEquipmentState;
    }

    public void SerializePlayerData(List<SlotData<EquipmentType>> datas){
        playerStatePresenter.SerializeModel(datas);
        equipmentPresenter.SerializeModel(datas);
    }

    //State 갱신
    #region State 갱신
    private void ApplyEquipmentState(Equipment pre, Equipment cur){
        playerStatePresenter.ApplyEquipment(pre, cur);
    }

    public void ApplyEffect(IStateEffect effect){
        playerStatePresenter.ApplyEffect(effect);
    }
    #endregion

    public void TogglePlayerDataView(){
        WindowController window = stateView.GetComponentInParent<WindowController>();

        bool isActive = !window.gameObject.activeSelf;
        window.gameObject.SetActive(isActive);
        
        if(isActive) stateView.UpdatePlayerStateView(currentPlayerState);
    }
}