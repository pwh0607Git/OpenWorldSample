using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType
{
    Head,           //defend
    Weapon,         //attack
    Cloth,          //defend
    Foot            //speed
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : ItemData
{
    public EquipmentType subType;
    public float value;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }

    //장비 아이템은 장착을 의미...
    public override void Use(/*GameObject player*/)
    {
        GameObject playerState;
        //참조 추가하기.
        switch (subType)
        {
            case EquipmentType.Head:
                Debug.Log("헤드 장착");
                break;
            case EquipmentType.Weapon:
                Debug.Log("무기 장착");
                break;
            case EquipmentType.Cloth:
                Debug.Log("옷 장착");
                break;
            case EquipmentType.Foot:
                Debug.Log("신발 장착");
                break;
        }
    }
}
