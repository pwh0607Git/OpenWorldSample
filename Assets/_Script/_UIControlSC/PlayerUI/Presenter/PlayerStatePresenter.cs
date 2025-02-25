using UnityEngine;

public class PlayerStatePresenter : MonoBehaviour
{
    private PlayerStateModel model;
    private PlayerStateView view;

    public PlayerStatePresenter(PlayerStateModel model, PlayerStateView view){
        this.model = model;
        this.view = view;

        model.OnModelChanged += ModelChangeHandler;
    }

    public void ModelChangeHandler(){
        Debug.Log("Model이 변경되었다! View를 Update하러 가자!");
        view.UpdateView(model.GetState());
    }
}