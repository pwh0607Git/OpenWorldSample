using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    PlayerController1 player;
    public UnityAction<AbilityFlag, bool> OnPerformedAttack;
    public UnityAction<AbilityFlag, bool> OnPerformedDamaged;
    public UnityAction<AbilityFlag, bool> OnPerformedDodged;
    public UnityAction OnPerformedRunning;

    public void SlashAttack(int index){
        var monsters = player.attackArea.attackableMonsterList;
        int damage = index == 0 ? 10 : 20;
        foreach(var monster in monsters){
            MonsterController monsterController = monster.GetComponent<MonsterController>();
            
            if(monsterController == null) continue;

            monsterController.TakeDamage(damage);
        }
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