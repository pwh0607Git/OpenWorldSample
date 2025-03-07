using UnityEngine;

public enum ItemType
{
    Equipment,
    Consumable,
    ETC
}

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Object/Item Data")]
public abstract class ItemData : ScriptableObject
{
    [Header("Information")]
    public string itemName;
    public string description;
    public float value;        
    public ItemType itemType;      

    [Header("Visual")]
    public Sprite icon;
    public GameObject model;
}

public enum ConsumableType
{
    HP,
    MP,
    SpeedUp
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Items/Consumable")]
public class ConsumableData : ItemData
{
    public ConsumableType subType;
}

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
    public State stateAddtive;
}