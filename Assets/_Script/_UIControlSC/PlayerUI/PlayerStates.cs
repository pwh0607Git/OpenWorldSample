using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class State
{
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

public class PlayerStates : MonoBehaviour
{
    public GameObject HP_Bar;
    public GameObject MP_Bar;

    private Image HP_Image;
    private Image MP_Image;

    [SerializeField] private State myState;

    private void Start()
    {
        StartCoroutine(Coroutine_InitMyState());
    }

    IEnumerator Coroutine_InitMyState()
    {
        while (myState == null)
        {
            myState = PlayerController.player.myState;
            yield return null;
        }

        HP_Image = HP_Bar.GetComponent<Image>();
        MP_Image = MP_Bar.GetComponent<Image>();
        myState.OnStateChanged += UpdateStateUI;
    }

    public void UpdateStateUI()
    {
        HP_Image.fillAmount = (float)myState.curHP / myState.maxHP;
        MP_Image.fillAmount = (float)myState.curMP / myState.maxMP;
    }
}