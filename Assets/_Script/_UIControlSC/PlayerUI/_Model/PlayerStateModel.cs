using System;
using JetBrains.Annotations;
using UnityEngine;

public class PlayerStateModel
{
    private PlayerState original_state;
    private PlayerState p_state;
    public event Action OnModelUpdated;
    public PlayerStateModel(){
        original_state = new PlayerState();
    }

    public PlayerStateModel(PlayerState state){
        original_state = state;
    }

    public PlayerState GetState() => original_state;
     
    public void UpdateModel(PlayerState newState){
        p_state = newState;
        OnModelUpdated?.Invoke();
    }
}

public class PlayerState{
    public int maxHp {get; private set;}
    public int maxMp {get; private set;}

    public int currentHp {get; private set;}
    public int currentMp {get; private set;}

    //Combat
    public int attack {get; private set;}
    public int defend {get; private set;}
    public float speed {get; private set;}

    public PlayerState(int maxHp = 100, int maxMp = 50)
    {
        this.maxHp = maxHp;
        this.maxMp = maxMp;
        this.currentHp = maxHp;
        this.currentMp = maxMp;
    }
    public void Heal(int amount)
    {
        currentHp = Mathf.Clamp(currentHp + amount, 0, maxHp);
    }
    
    public void RestoreMana(int amount)
    {
        currentMp = Mathf.Clamp(currentMp + amount, 0, maxMp);
    }

    public void TakeDamage(int damage){
        currentHp-= damage;
    }

    public PlayerState EquipItem(Equipment item){
        PlayerState newState = this;

        PlayerState add = ((EquipmentData)item.data).stateAddtive;

        newState.maxHp += add.maxHp;
        newState.attack += add.attack;
        newState.defend += add.defend;
        newState.speed *= add.speed;             //speed는 곱연산!

        return newState;
    }

    public PlayerState DetachItem(Equipment item){
        PlayerState newState = this;

        PlayerState add = ((EquipmentData)item.data).stateAddtive;

        newState.maxHp -= add.maxHp;
        newState.attack -= add.attack;
        newState.defend -= add.defend;
        newState.speed /= add.speed;             //speed는 곱연산!

        return newState;
    }   
}