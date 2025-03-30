using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    // public PlayerUIPresenter uIPresenter;
    [ReadOnly] public CharacterController controller;
    [ReadOnly] public AbilityController abilityController;
    [ReadOnly] public Animator animator;   
    private AttackArea attackArea;
    private AnimationEventListener eventListener;
    [SerializeField] AbilityFlag initialAbilities;
    [SerializeField] List<AbilityData> staticDatas;

    [SerializeField] AbilityFlag currentActivatedAbilities;
    [ReadOnly] public bool isGrounded;

    void Awake()
    {
        TryGetComponent(out controller);
        TryGetComponent(out animator);
        TryGetComponent(out abilityController);
        TryGetComponent(out eventListener);

        // uIPresenter = GetComponentInChildren<PlayerUIPresenter>();
        attackArea = GetComponentInChildren<AttackArea>();
    }

    void Start()
    {
        SetAbilities();
        eventListener.OnPerformedAttack += SetAbilityFlag;
        eventListener.OnPerformedDamaged += SetAbilityFlag;
        eventListener.OnPerformedDodged += SetAbilityFlag;
    
    }

    void SetAbilityFlag(AbilityFlag flag, bool immediate){
        if(immediate) currentActivatedAbilities.Add(flag, null);
        else currentActivatedAbilities.Remove(flag, null);
    }

    void SetAbilities(){
        abilityController.Add(AbilityFlag.Move, new AbilityMove(new PlayerState(), this), true);
        abilityController.Add(AbilityFlag.Attack, new AbilityAttack(new PlayerState(), this, attackArea), true);
        abilityController.Add(AbilityFlag.Damaged, new AbilityDamaged(new PlayerState(), this), true);
        abilityController.Add(AbilityFlag.Dodge, new AbilityDodge(staticDatas.Find(d=> d.flag == AbilityFlag.Dodge) as AbilityDodgeData, this), true);
    }

    void Update()
    {
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            abilityController.Activate(AbilityFlag.Attack);
        }

        if(Input.GetKeyDown(KeyCode.Space) && !currentActivatedAbilities.Has(AbilityFlag.Dodge)){
            abilityController.Activate(AbilityFlag.Dodge);
        }
        //Test
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage){
        abilityController.Activate(AbilityFlag.Damaged);            //Damaged의 경우에는 애니메이션 실행만 수행한다. => 애니메이션 이벤트를 통해 PlayerState를 변경.
    }
    
    public void UpdatePlayerState(PlayerState p_state){
        abilityController.UpdatePlayerState(p_state);
    }
}