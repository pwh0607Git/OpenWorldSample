using System.Collections;
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
    //아이템 확률 테이블.
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
    
    //이동
    public float moveSpeed;
    public float movingArea = 100f;             //이동 가능 범위.

    //공격
    public float detectionRadius = 15f;         // 골렘이 캐릭터를 인식하는 범위
    public float attackableRadius = 3f;             // 공격을 시작하는 범위
    public float attackDamageRadius = 5f;       // 공격 피해를 주는 범위
    public float attackCooldown = 2f;           // 공격 후 대기 시간
    public int attackPower;

    [Header("Visuals")]
    public GameObject monsterPrefab;        // 몬스터 3D 모델 프리팹

    [Header("Props")]
    public List<GameObject> basicLoot;      //기본적으로 가지고 있는 전리품.
    public List<LootEntry> randomLoot;           //전리품

    //유효성 검사 코드.
}