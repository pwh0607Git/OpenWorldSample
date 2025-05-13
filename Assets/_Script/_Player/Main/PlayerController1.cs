using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public PlayerUIPresenter uIPresenter;
    [ReadOnly] public CharacterController controller;
    [ReadOnly] public AbilityController abilityController;
    [ReadOnly] public Animator animator;   
    public AttackArea attackArea;
    private EventListener eventListener;

    [ReadOnly] public bool isGrounded;
    
    //Test Code
    [SerializeField] List<AbilityData> datas;

    void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out animator);
        TryGetComponent(out abilityController);
        TryGetComponent(out eventListener);

        uIPresenter = GetComponentInChildren<PlayerUIPresenter>();
        attackArea = GetComponentInChildren<AttackArea>();

        //test
        SetAbility(datas);
    }

    public void SetAbility(List<AbilityData> abilityDatas){
        foreach(var data in abilityDatas){
            abilityController.Add(data, true);
        }
    }

    void Update()
    {
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage, IStateEffect effect = null){
        abilityController.Activate(AbilityFlag.Damaged);            //Damaged의 경우에는 애니메이션 실행만 수행한다. => 애니메이션 이벤트를 통해 PlayerState를 변경.
    }
    
    public void UpdatePlayerState(PlayerState p_state){
        abilityController.UpdatePlayerState(p_state);
    }
}