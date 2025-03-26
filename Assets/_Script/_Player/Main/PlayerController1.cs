using System.Collections;
using CustomInspector;
using UnityEngine;

public class PlayerController1 : MonoBehaviour
{
    public PlayerUIPresenter uIPresenter;
    [ReadOnly] public CharacterController controller;
    [ReadOnly] public AbilityController abilityController;
    [ReadOnly] public Animator animator;   
    private AttackArea attackArea;
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
        abilityController.Add(AbilityFlag.Move, new AbilityMove(new PlayerState(), this), true);
        abilityController.Add(AbilityFlag.Attack, new AbilityAttack(new PlayerState(), this, attackArea), true);
        abilityController.Add(AbilityFlag.Damaged, new AbilityDamaged(new PlayerState(), this), true);
    }

    void Update()
    {
        InputKeyboard();
    }

    void InputKeyboard(){
        if(Input.GetKeyDown(KeyCode.LeftControl)){
            abilityController.Activate(AbilityFlag.Attack);
        }

        //Test
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage){
        Debug.Log($"Damage :{damage}");
        abilityController.Activate(AbilityFlag.Damaged);
        // uIPresenter.ApplyEffect(EffectFactory.CreateEffect(EffectType.Damage, damage));

        //실행 후에 잠시동안 데미지를 받지 않도록 하기 => 끔살 방지
        abilityController.Remove(AbilityFlag.Damaged);
        float duration = 2f;
        StartCoroutine(ResetAbility(AbilityFlag.Damaged, duration));
    }

    IEnumerator ResetAbility(AbilityFlag flag, float duration){
        yield return new WaitForSeconds(duration);
        abilityController.Add(flag, new AbilityDamaged(new PlayerState(), this));
    }

    public void UpdatePlayerState(PlayerState p_state){
        abilityController.UpdatePlayerState(p_state);
    }
}