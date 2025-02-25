using System;

public class PlayerStateModel
{
    PlayerState playerState;
    public PlayerStateModel(){
        this.playerState = null;
    }

    public void SerializePlayerState(PlayerState playerState){
        this.playerState = playerState;
    }
}

public class PlayerState{
    public int maxHP;
    public int curHP;
    public int maxMP;
    public int curMP;

    public float speed;
    public float defend;
    public float attack;

    public Action OnStateChanged;

    public void EquipItem(Equipment item)
    {
        switch (item.subType)
        {
            case EquipmentType.Head:
                {
                    defend += item.value;
                    break;
                }
            case EquipmentType.Weapon:
                {
                    attack += item.value;
                    break;
                }
            case EquipmentType.Cloth:
                {
                    maxHP += (int)item.value;
                    break;
                }
            case EquipmentType.Foot:
                {
                    defend += item.value;
                    break;
                }
        }
        NotifyStateChange();
    }

    public void DetachItem(Equipment item)
    {
        switch (item.subType)
        {
            case EquipmentType.Head:
            {
                defend -= item.value;
                break;
            }
            case EquipmentType.Weapon:
            {
                attack -= item.value;
                break;
            }
            case EquipmentType.Cloth:
            {
                maxHP -= (int)item.value;
                break;
            }
            case EquipmentType.Foot:
            {
                speed -= item.value;
                break;
            }
        }
        NotifyStateChange();
    }

    public void UesConsumable(Consumable itemData)
    {
        switch (itemData.subType)
        {
            case ConsumableType.HP:
                {
                    curHP += (int)itemData.value;
                    if (curHP >= maxHP) curHP = maxHP;
                    break;
                }
            case ConsumableType.MP:
                {
                    curMP += (int)itemData.value;
                    if (curMP >= maxMP) curMP = maxMP;
                    break;
                }
            case ConsumableType.SpeedUp:
                {
                    speed += itemData.value;
                    float duration = 10f;
                    PlayerController.uiController.OnBuffItem(itemData, duration);
                    break;
                }
        }
        NotifyStateChange();
    }

    public void NotifyStateChange()
    {
        OnStateChanged?.Invoke();
    }
}
