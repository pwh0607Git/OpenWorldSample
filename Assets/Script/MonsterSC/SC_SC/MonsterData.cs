using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MushRoom
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Basic Attributes")]
    public string monsterName;
    public MonsterType monsterType;
    public int HP;
    public int attackPower;
    public float moveSpeed;

    [Header("Visuals")]
    public GameObject monsterPrefab; // ¸ó½ºÅÍ 3D ¸ðµ¨ ÇÁ¸®ÆÕ
}