using CustomInspector;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public PlayerUIPresenter uIPresenter;
    [ReadOnly] public CharacterController controller;
    [ReadOnly] public Animator animator;   
    [ReadOnly] public AbilityController abilityController;
    [SerializeField] AttackArea attackArea;
    [SerializeField] AbilityFlag initialAbilities;
    [ReadOnly] public bool isGrounded;

    void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out animator);
        TryGetComponent(out abilityController);
        uIPresenter = GetComponentInChildren<PlayerUIPresenter>();
        attackArea = GetComponentInChildren<AttackArea>();
    }

    void Start()
    {
        SetAbilities();
    }

    void SetAbilities(){
        abilityController.Add(AbilityFlag.Move, new AbilityMove(new PlayerState(), this));              // 나중에 저장되어있는 Player State ref 변경예정.
        abilityController.Add(AbilityFlag.Attack, new AbilityAttack(new PlayerState(), this, attackArea));
    }

    void Update()
    {
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            abilityController.Activate(AbilityFlag.Attack);
        }
    }

    public void UpdatePlayerState(PlayerState p_state){
        abilityController.UpdatePlayerState(p_state);
    }
}