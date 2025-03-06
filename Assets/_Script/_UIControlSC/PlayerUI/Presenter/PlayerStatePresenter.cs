using UnityEngine;

public class PlayerStatePresenter
{
    private PlayerStateModel model;
    private PlayerStateView view;

    public PlayerStatePresenter(PlayerStateModel model, PlayerStateView view){
        this.model = model;
        this.view = view;

        model.OnModelUpdated += ModelChangeHandler;
    }

    public void ModelChangeHandler(){
        Debug.Log($"{GetType()} : Model이 변경되었다! View를 Update하러 가자!");
        view.UpdateView(model.GetState());
    }

    public void UseItem(ConsumableData data){
        float value = data.value;
        Debug.Log($"[{data.name}] 아이템 사용!");

        PlayerState state = model.GetState();
        model.UpdateModel(state);
    }

    public void UseBuffItem(ConsumableData data){
        Debug.Log($"[{data.name}] 버프 ON !!");
    }

    public void EquipItem(EquipmentData data){
        Debug.Log($"[{data.name}] 장착 !!");
        //장착할 아이템의 스탯 값을 model에서 가산.
    }

    public void DetachItem(EquipmentData data){
        //장착할 아이템의 스탯 값을 model에서 가감.
    } 
}