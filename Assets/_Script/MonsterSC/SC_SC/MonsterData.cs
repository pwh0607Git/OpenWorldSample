using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    MushRoom,
    Golem,
    Bat
}

[System.Serializable]
public class LootEntry
{
    public ItemData itemData;

    public float dropRate;

    public LootEntry(ItemData itemData, float dropRate)
    {
        this.itemData = itemData;
        this.dropRate = dropRate;
    }
}

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data")]
public class MonsterData : ScriptableObject
{
    [Header("Basic Attributes")]
    public string monsterName;
    public MonsterType monsterType;
    public int HP;
    public float moveSpeed;
    public float movableArea = 50f;             //�̵� ���� ����.
    public float movingAreaRedius = 30f;        //idle ���� �̵��ϴ� ����.

    public float detectionRadius = 15f;         // 감지 범위
    public float chasingRadius = 20f;           
    public float attackableRadius = 3f;         // 공격을 시작하는 범위
    public float attackDamageRadius = 5f;       // 공격을 했을 때, 이 범위에 있으면 공격성공.
    public float attackCooldown = 2f;           // 공격 대기시간.
    public int attackPower;

    [Header("Visuals")]
    public GameObject monsterPrefab;

    [Header("Loots")]
    public List<ItemData> basicLoot;       
    public List<LootEntry> randomLoot;    
}