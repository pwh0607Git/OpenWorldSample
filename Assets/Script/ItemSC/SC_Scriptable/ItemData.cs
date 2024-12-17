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
    [Header("Information")]
    public string itemName;
    public string description;
    public float value;        
    public ItemType itemType;      

    [Header("Visual")]
    public Sprite icon;
    public GameObject model;

    public abstract void Use();
}