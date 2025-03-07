using System;
using UnityEngine;

public class PlayerStateModel
{
    private PlayerState p_state;
    public event Action OnModelUpdated;
    public PlayerStateModel(){
        p_state = new PlayerState();
    }

    public PlayerState GetState() => p_state;
     
    public void UpdateModel(PlayerState state){
        p_state = state;
        OnModelUpdated?.Invoke();
    }
}

public class PlayerState{
    public int maxHp {get; private set;}
    public int maxMp {get; private set;}

    public int currentHp {get; private set;}
    public int currentMp {get; private set;}

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
}