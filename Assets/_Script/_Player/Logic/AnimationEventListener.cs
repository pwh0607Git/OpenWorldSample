using UnityEngine;
using UnityEngine.Events;

public class AnimationEventListener : MonoBehaviour
{
    public UnityAction<bool> OnPerformedAttack;
    public UnityAction<bool> OnPerformedDamaged;
    public UnityAction<bool> OnPerformedDodged;
    public UnityAction OnPerformedRunning;

    public void AttackStart(){
        OnPerformedAttack?.Invoke(true);
    }

    public void AttackEnd(){
        OnPerformedAttack?.Invoke(false);
    }

    public void DamagedStart(){
        OnPerformedDamaged?.Invoke(true);
    }

    public void DamagedEnd(){
        OnPerformedDamaged?.Invoke(false);
    }

    public void DodgeStart(){
        OnPerformedDodged?.Invoke(true);
    }

    public void DodgeEnd(){
        OnPerformedDodged?.Invoke(true);
    }
}