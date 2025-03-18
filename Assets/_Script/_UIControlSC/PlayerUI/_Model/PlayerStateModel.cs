using System;
using UnityEngine;

public class PlayerStateModel
{
    PlayerState p_state;
    public event Action OnModelUpdated;
    public PlayerStateModel(){
        p_state = new PlayerState();
    }

    public PlayerStateModel(PlayerState state){
        p_state = state;
    }

    public PlayerState GetState() => p_state;
    
    public void EquipItem(Equipment equipment){
        EquipmentData data = equipment.data as EquipmentData;
        p_state.ApplyBonus(data.state);
        OnModelUpdated?.Invoke();
    }

    public void UnequipItem(Equipment equipment){
        EquipmentData data = equipment.data as EquipmentData;
        p_state.RemoveBonus(data.state);
        OnModelUpdated?.Invoke();
    }

    public void ApplyEffect(IStateEffect effect){
        Debug.Log($"StateModel : {effect} 적용!");
        effect.Apply(p_state);
        OnModelUpdated?.Invoke();
    }
}

public class PlayerState{
    public State base_State {get; private set;}
    public State bonus_State{get; private set;}
    public State state => base_State + bonus_State;

    public int currentHp {get; private set;}
    public int currentMp {get; private set;}
    public int level {get; private set;}

    public PlayerState()
    {
        base_State = new State(100, 50, 10, 10, 30f);
        bonus_State = new State();
        currentHp = state.hp;
        currentMp = state.mp;
        level = 1;
    }

    public void Heal(int amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, base_State.hp + bonus_State.hp);
    }
    
    public void RestoreMana(int amount)
    {
        currentMp = Mathf.Clamp(currentMp + amount, 0, base_State.mp);
    }

    public void ApplyDamage(int damage){
        currentHp -= damage;
    }

    public void CostMp(int cost){
        currentMp -= cost;
    }

    public void ApplyBonus(State bonusState){
        bonus_State += bonusState;
    }

    public void RemoveBonus(State bonusState){
        bonus_State -= bonusState;
    }   

    public void LevelUp(){
        level++;
    }
}


[Serializable]
public class State{
    public int hp;
    public int mp;
    public int attack;
    public int defend;
    public float speed;

    public State(int hp = 0, int mp = 0, int attack = 0, int defend = 0, float speed = 0){
        this.hp = hp;
        this.mp = mp;
        this.attack = attack;
        this.defend = defend;
        this.speed = speed;
    }

    public State(State state){
        this.hp = state.hp;
        this.mp = state.mp;
        this.attack = state.attack;
        this.defend = state.defend;
        this.speed = state.speed;
    }

    public static State operator +(State a, State b)
    {
        return new State(
            a.hp + b.hp,
            a.mp + b.mp,
            a.attack + b.attack,
            a.defend + b.defend,
            a.speed + b.speed
        );
    }

    public static State operator -(State a, State b)
    {
        return new State(
            Mathf.Clamp(a.hp - b.hp, 0, a.hp - b.hp),
            Mathf.Clamp(a.mp - b.mp, 0, a.mp - b.mp),
            Mathf.Clamp(a.attack - b.attack, 0, a.attack - b.attack),
            Mathf.Clamp(a.defend - b.defend, 0, a.defend - b.defend),
            Mathf.Clamp(a.speed - b.speed, 0, a.speed - b.speed)
        );
    }
}
