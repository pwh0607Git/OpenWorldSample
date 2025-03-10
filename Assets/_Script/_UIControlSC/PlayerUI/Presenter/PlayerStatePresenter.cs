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

    public void UseItem(Item item){
        item.Use(model.GetState());
    }

    public void EquipItem(EquipmentData data){
        Debug.Log($"[{data.name}] 장착 !!");
        //장착할 아이템의 스탯 값을 model에서 가산.
    }

    public void DetachItem(EquipmentData data){
        Debug.Log($"[{data.name}] 해제 !!");
        //장착할 아이템의 스탯 값을 model에서 가감.
    }

    public void TakeDamage(int damage){
        PlayerState state = model.GetState();
        state.TakeDamage(damage);
        Debug.Log($"CurrentHp : {state.currentHp}");
        model.UpdateModel(state);
    }
}