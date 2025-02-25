using System;
using UnityEngine;

public class PlayerStateModel
{
    private PlayerState p_state;
    public event Action OnModelChanged; 
    public PlayerStateModel(){
        p_state = new PlayerState();
    }

    public PlayerState GetState() => p_state;
     
    public void UpdateModel(PlayerState state){
        p_state = state;
        OnModelChanged?.Invoke();
    }
}

[Serializable]
public class PlayerState{
    public int maxHP {get; private set;}
    public int curHP;
    public int maxMP {get; private set;}
    public int curMP;

    public float speed;
    public float defend;
    public float attack;

    public event Action OnStateChanged;

public void EquipItem(Equipment item) {
        switch (item.subType) {
            case EquipmentType.Head: defend += item.value; break;
            case EquipmentType.Weapon: attack += item.value; break;
            case EquipmentType.Cloth: maxHP += (int)item.value; break;
            case EquipmentType.Foot: defend += item.value; break;
        }
        NotifyStateChange();
    }

    public void DetachItem(Equipment item) {
        switch (item.subType) {
            case EquipmentType.Head: defend -= item.value; break;
            case EquipmentType.Weapon: attack -= item.value; break;
            case EquipmentType.Cloth: maxHP -= (int)item.value; break;
            case EquipmentType.Foot: speed -= item.value; break;
        }
        NotifyStateChange();
    }

    public void UseConsumable(Consumable itemData) {
        switch (itemData.subType) {
            case ConsumableType.HP: curHP = Mathf.Min(curHP + (int)itemData.value, maxHP); break;
            case ConsumableType.MP: curMP = Mathf.Min(curMP + (int)itemData.value, maxMP); break;
            case ConsumableType.SpeedUp:
                speed += itemData.value;
                float duration = 10f;
                PlayerController.uiController.OnBuffItem(itemData, duration);
                break;
        }
        NotifyStateChange();
    }

    private void NotifyStateChange() => OnStateChanged?.Invoke();
}