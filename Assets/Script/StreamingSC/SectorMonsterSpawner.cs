using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorMonsterSpawner : MonoBehaviour
{
    // Scene 내부에 존재하며 Scene이 활성화 되었을때 대기하고있다.
    // 캐릭터와의 거리가 1Sector 정도 왔을 때, 몬스터를 활성화 한다.
    public static SectorMonsterSpawner localInstance;

    private void Awake()
    {
        localInstance = this;
    }

    public GameObject monsterPrefab;
    //public Transform[] spawnPrefab;
    public int spawnCount;          //섹터별로 별도로 세팅.

    private Queue<GameObject> monsterPool;
    public Vector2Int sectorVec;

    void Start()
    {
        //초기화 횟수 필요.    
        monsterPool = new Queue<GameObject>();
        SpawnMonster();
    }

    private void OnEnable()
    {
        //활성화 된 경우... 중앙 처리 오브젝트에 SC 참조를 넘긴다.
        MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, localInstance);
    }

    private void OnDisable()
    {
        MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);       
    }

    public void SpawnMonster()
    {
        //test용으로 현재 스폰서의 위치에 소환한다 [스폰서의 중앙]
        GameObject monster = Instantiate(monsterPrefab, transform.position, transform.rotation);
    }
}