using Unity.VisualScripting;
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

    public void ApplyEffect(IStateEffect effect){
    // model.ApplyEffect(effect);
    }

    public void EquipItem(Equipment equipment){
        Debug.Log($"[{equipment}] 장착 !!");
        model.EquipItem(equipment);
    }

    public void UnequipItem(Equipment equipment){    
        Debug.Log($"[{equipment}] 해제 !!");
        model.EquipItem(equipment);
    }

    public void TakeDamage(int damage){
        model.TakeDamage(damage);
    }
}