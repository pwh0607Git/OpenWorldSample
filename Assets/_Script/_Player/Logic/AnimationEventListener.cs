using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    public UnityAction<AbilityFlag, bool> OnPerformedAttack;
    public UnityAction<AbilityFlag, bool> OnPerformedDamaged;
    public UnityAction<AbilityFlag, bool> OnPerformedDodged;
    public UnityAction OnPerformedRunning;

    public void AttackStart(){
        OnPerformedAttack?.Invoke(AbilityFlag.Attack, true);
    }

    public void AttackEnd(){
        OnPerformedAttack?.Invoke(AbilityFlag.Attack, false);
    }

    public void DamagedStart(){
        OnPerformedAttack?.Invoke(AbilityFlag.Damaged, true);
    }

    public void DamagedEnd(){
        OnPerformedAttack?.Invoke(AbilityFlag.Damaged, false);
    }

    public void DodgeStart(){
        OnPerformedAttack?.Invoke(AbilityFlag.Dodge, true);
    }

    public void DodgeEnd(){
        OnPerformedAttack?.Invoke(AbilityFlag.Dodge, false);
    }
}