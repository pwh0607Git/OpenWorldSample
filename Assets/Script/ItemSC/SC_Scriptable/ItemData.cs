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
    public string itemName;         //이름
    public string description;      //설명
    public Sprite icon;             //아이콘
    public ItemType itemType;       //아이템 type
    public abstract void Use(/*GameObject player*/);
}
