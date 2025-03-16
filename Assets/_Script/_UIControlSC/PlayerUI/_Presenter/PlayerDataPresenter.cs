using UnityEngine;

public class PlayerDataPresenter
{
    private PlayerStateModel model;
    private PlayerDataView dataView;
    private PlayerHealthbarView healthbarView;

    public PlayerDataPresenter(PlayerStateModel model, PlayerDataView dataView, PlayerHealthbarView healthbarView){
        this.model = model;
        this.dataView = dataView;
        this.healthbarView = healthbarView;
        
        model.OnModelUpdated += ModelChangeHandler;
    }

    public void ModelChangeHandler(){
        Debug.Log($"{GetType()} : Model이 변경되었다! View를 Update하러 가자!");
        dataView.UpdatePlayerDataView(model.GetState());
        healthbarView.UpdateView(model.GetState());
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
}