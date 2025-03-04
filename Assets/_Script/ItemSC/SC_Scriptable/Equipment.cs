using UnityEngine;

public enum EquipmentType
{
    Head,                           
    Weapon,                         
    Cloth,                        
    Foot                           
}

[CreateAssetMenu(fileName = "Equipment", menuName = "Items/Equipment")]
public class Equipment : ItemData
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

    public override void Use(){
        // 슬롯에서 값 처리하기.
    }
}