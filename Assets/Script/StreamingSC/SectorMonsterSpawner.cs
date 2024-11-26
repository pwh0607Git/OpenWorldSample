using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class SectorMonsterSpawner : MonoBehaviour
{
    //해당 지역에 있는 몬스터 List
    public List<Monster> monsterObjects = new List<Monster>();
    
    //sector 마다 몬스터 소환 위치를 Inspector를 통해 따로 설정하기!
    public List<GameObject> spawnPositionGroup;
    //Object -> child[,,,] -> 해당 위치에 소환.
    
    //해당 스폰지역의 몬스터 소환 상태.
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

            GameObject[] group= posGroup.GetComponentsInChildren<GameObject>();
            foreach(GameObject spawnPos in group)
            {
                monsterInPosition[spawnPos.transform] = null;
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
            GameObject[] group = posGroup.GetComponentsInChildren<GameObject>();
            foreach (GameObject spawnPos in group)
            {
                if (monsterInPosition[spawnPos.transform] == null)
                {
                    GameObject monster = objectPooling.GetObject(monsterType.prefab.tag, spawnPos.transform.position, Quaternion.identity);
                    if (monster != null)
                    {
                        monsterInPosition[spawnPos.transform] = monster;
                        monsterType.PlusCurCount();

                        // 인스턴스화 된 몬스터에 사망 이벤트 등록
                        MonsterController controller = monster.GetComponent<MonsterController>();
                        controller.OnMonsterDeath += () => HandleMonsterDeath(spawnPos.transform, monsterType);
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
            //position을 체크하여 알아서 몬스터 소환 기능을 수행한다.
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
        this.inCount = inCount;             //현재 섹터에 해당 몬스터가 몇기가 있어야하는지...
        this.curCount = 0;
        this.poolCount = poolCount;         //오브젝트 풀링용
    }

    public void MinusCurCount() => curCount--;
    public void PlusCurCount() => curCount++;
    public bool CheckInCount() => curCount < inCount;
}

//차후 사용예정.
public class Boss : Monster
{
    public Boss(int HP, int size, int damage, float speed, GameObject prefab, int count) : base(size, HP, damage, speed, prefab, 1, count) { }
}