using UnityEngine;

public class PlayerStatePresenter : MonoBehaviour
{
    private PlayerStateModel model;
    private PlayerStateView view;

    public PlayerStatePresenter(PlayerStateModel model, PlayerStateView view){
        this.model = model;
        this.view = view;
    }
}