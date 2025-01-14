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
    public GameObject item;

    public float dropRate;

    public LootEntry(GameObject item, float dropRate)
    {
        this.item = item;
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
    public float movableArea = 50f;            //�̵� ���� ����.
    public float movingAreaRedius = 30f;        //idle ���� �̵��ϴ� ����.

    public float detectionRadius = 15f;         // ���� ĳ���͸� �ν��ϴ� ����
    public float attackableRadius = 3f;             // ������ �����ϴ� ����
    public float attackDamageRadius = 5f;       // ���� ���ظ� �ִ� ����
    public float attackCooldown = 2f;           // ���� �� ��� �ð�
    public int attackPower;

    [Header("Visuals")]
    public GameObject monsterPrefab;        // ���� 3D �� ������

    [Header("Props")]
    public List<GameObject> basicLoot;      //�⺻������ ������ �ִ� ����ǰ.
    public List<LootEntry> randomLoot;           //����ǰ

    //��ȿ�� �˻� �ڵ�.
}