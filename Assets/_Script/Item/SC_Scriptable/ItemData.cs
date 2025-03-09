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