using UnityEngine;

public enum EquipmentType
{
    Head,                           
    Weapon,                         
    Cloth,                        
    Foot                           
}

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Items/Equipment")]
public class EquipmentData : ItemData
{
    public EquipmentType subType;

    public int maxHpBonus = 0;
    public int attackBonus = 0;
    public int defendBonus = 0;
    public float speedBonus = 0;

    private void OnEnable()
    {
        itemType = ItemType.Equipment;
    }
}