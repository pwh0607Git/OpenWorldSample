using UnityEngine;

public enum EquipmentType
{
    Head,                           
    Weapon,                         
    Cloth,                        
    Foot                           
}

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Items/EquipmentData")]
public class EquipmentData : ItemData
{
    public EquipmentType subType;
    public State stateAddtive;
}