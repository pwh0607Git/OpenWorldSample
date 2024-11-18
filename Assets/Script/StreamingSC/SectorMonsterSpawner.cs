using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMonsterSpawner : MonoBehaviour
{
    // Scene 내부에 존재하며 Scene이 활성화 되었을때 대기하고있다.
    // 캐릭터와의 거리가 1Sector 정도 왔을 때, 몬스터를 활성화 한다.
    public static SectorMonsterSpawner localInstance;
    
    public static ObjectPooling ObjectPooling;
    public List<GameObject> spawnedMonster;


    private void Awake()
    {
        localInstance = this;
        spawnedMonster = new List<GameObject>();
    }

    public int spawnCount;          //섹터별로 별도로 세팅.

    public Vector2Int sectorVec;
    
    void Start()
    {
        ObjectPooling = GetComponent<ObjectPooling>();
    }

    private void OnEnable()
    {
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, localInstance);
    }

    private void OnDisable()
    {
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);       
    }

    public void SpawnMonster()
    {
        GameObject monster = ObjectPooling.GetObject(transform.position, transform.rotation);   
        spawnedMonster.Add(monster);
    }

    public void DespawnMonster()
    {
        if (spawnedMonster.Count > 0)
        {
            foreach (GameObject monster in spawnedMonster)
            {
                ObjectPooling.ReturnObject(monster);
            }
        }
    }
}