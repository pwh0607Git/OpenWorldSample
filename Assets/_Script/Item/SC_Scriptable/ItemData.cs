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

    [Header("Visual")]
    public Sprite icon;
    public GameObject model;
}

public enum ConsumableType
{
    HP,
    MP,
    Attackup
}

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Items/ConsumableData")]
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

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Items/EquipmentData")]
public class EquipmentData : ItemData
{
    public EquipmentType subType;
    public State stateAddtive;
}