using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SectorMonsterSpawner : MonoBehaviour
{
    public List<Monster> monsterObjects = new List<Monster>();
    public List<GameObject> spawnPositionGroup;
    private Dictionary<Transform, GameObject> monsterInPosition= new Dictionary<Transform, GameObject>();    
    private ObjectPooling objectPooling;

    private bool isSpawning;
    public Vector2Int sectorVec;

    private void Awake()
    {
        objectPooling = GetComponent<ObjectPooling>();

        foreach (GameObject posGroup in spawnPositionGroup)
        {
            int ran = Random.Range(0, 360);
            posGroup.transform.Rotate(0, ran, 0);

            Transform[] group = posGroup.GetComponentsInChildren<Transform>();
            foreach (Transform spawnPos in group)
            {
                if (spawnPos != posGroup.transform)
                {
                    monsterInPosition[spawnPos] = null;
                }
            }
        }

        objectPooling.InitializeMonsterPool(monsterObjects);
    }

    void Start()
    {
        isSpawning = false;
    }

    private void SpawnInitialMonsters()
    {
        foreach (Monster monsterType in monsterObjects)
        {
            while (monsterType.CheckInCount()) SpawnMonster(monsterType);
        }
    }

    public void SpawnMonster(Monster monsterType)
    {
        foreach (GameObject posGroup in spawnPositionGroup)
        {
            Transform[] group = posGroup.GetComponentsInChildren<Transform>().Where(t => t != posGroup.transform).ToArray();

            foreach (Transform spawnPos in group)
            {
                if (monsterInPosition[spawnPos] == null)
                {
                    GameObject monster = objectPooling.GetObject(monsterType.prefab.tag, spawnPos.position, Quaternion.identity);
                    if (monster != null)
                    {
                        monsterInPosition[spawnPos] = monster;
                        monsterType.PlusCurCount();
                    }
                    return;
                }
            }
        }
    }

    void HandleMonsterDeath(Transform position, Monster monsterType)
    {
        monsterInPosition[position] = null;
        monsterType.MinusCurCount();
        objectPooling.ReturnObject(gameObject);
    }

    public void DespawnMonsterTotal()
    {
        foreach(var monster in monsterInPosition)
        {
            if(monster.Value != null)
            {
                objectPooling.ReturnObject(monster.Value);
            }
        }
        monsterInPosition.Clear();
    }

    public void OnPlayerEnter()
    {
        if (!isSpawning)
        {
            SpawnInitialMonsters();             
            isSpawning = true;
        }
    }

    public void OnPlayerExit()
    {
        if (isSpawning)
        {
            DespawnMonsterTotal();
            isSpawning = false;
        }
    }

    private void OnEnable()
    {
        isSpawning = false;
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, this);
    }

    private void OnDisable()
    {
        isSpawning = false;
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);
    }
}

[System.Serializable]
public class Monster
{
    public int HP;
    public int damage;
    public float speed;
    public int size;
    public GameObject prefab;
    public int inCount;
    private int curCount;
    public int poolCount;

    public Monster(int HP, int size, int damage, float speed, GameObject prefab, int inCount, int poolCount)
    {
        this.HP = HP;
        this.damage = damage;
        this.speed = speed;
        this.size = size;
        this.prefab = prefab;
        this.inCount = inCount;             //���� ���Ϳ� �ش� ���Ͱ� ��Ⱑ �־���ϴ���...
        this.curCount = 0;
        this.poolCount = poolCount;         //������Ʈ Ǯ����
    }

    public void MinusCurCount() => curCount--;
    public void PlusCurCount() => curCount++;
    public bool CheckInCount() => curCount < inCount;
}
