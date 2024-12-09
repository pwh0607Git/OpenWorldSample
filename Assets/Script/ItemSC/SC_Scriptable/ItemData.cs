using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public enum ItemType
{
    Equipment,
    Consumable,
    ETC
}

[CreateAssetMenu(fileName = "Item Data", menuName = "Scriptable Object/Item Data")]
public abstract class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public float value;
    public Sprite icon;           
    public ItemType itemType;      
    public abstract void Use();
}