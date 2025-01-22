using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public class MonsterSpawnEntry
{
    public MonsterData monsterData;
    public int count;
}

[System.Serializable]
public class MonsterSpawnPoint{
    public Transform point;
    public List<MonsterSpawnEntry> monsterSpawnTable;

    private Dictionary<MonsterData, int> countInPoint;

    public Dictionary<MonsterData, int> GetMonsterCountTable(){
        if(countInPoint == null){
            countInPoint = new Dictionary<MonsterData, int>();
            foreach(var entry in monsterSpawnTable){
                if(!countInPoint.ContainsKey(entry.monsterData)){
                    countInPoint.Add(entry.monsterData, entry.count);
                }
            }
        }
        return countInPoint;
    }
}

public class SectorMonsterSpawner : MonoBehaviour
{
    /*
        설계
        1. monsterDataList, 
        2. 특정 Empty GameObject를 통해 몬스터가 소환되는 지점을 설정(Random Circle사용(Vector2 => y축 무시)를 기준으로 몬스터가 소환되는 정확한 지점 설정하기)
        3. 해당 지점마다 소환되는 몬스터가 각각 할당 되어있다. 

        4. Player가 특정 지점에 가까이 갔을 때 소환하도록 설정하기 => 이건 mapStreaming에서 할당
    */
    public List<MonsterSpawnPoint> spawnPoints;
    private ObjectPooling objectPooling;
    private bool isSpawning;
    public Vector2Int sectorVec;

    [SerializeField] GameObject monsterStateUIPrefab;
    private void Awake()
    {
        objectPooling = GetComponent<ObjectPooling>();
    }

    [SerializeField] float spawnRadius;

    [Button("SpawnButton")]
    IEnumerator SpawnInitialMonsters(){
        foreach(var point in spawnPoints){
            Transform spawnPoint = point.point;
            foreach(var entry in point.monsterSpawnTable){
                for(int i=0;i<entry.count;i++){
                    Vector2 ran = UnityEngine.Random.insideUnitCircle * spawnRadius;
                    Vector3 spawnPosition = new Vector3(spawnPoint.position.x + ran.x, 0, spawnPoint.position.z + ran.y);

                    MonsterData monsterData = entry.monsterData;
                    GameObject monster = Instantiate(monsterData.monsterPrefab, spawnPosition, Quaternion.identity);
                    MonsterController monsterController = monster.GetComponent<MonsterController>();
                    if (monsterController != null)
                    {
                        monsterController.MonsterData = monsterData;
                    }
                    Instantiate(monsterStateUIPrefab, monster.transform);
                }
            }
        }
        return null;
    }

    void Start()
    {
        isSpawning = false;
        StartCoroutine(SpawnInitialMonsters());
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
            // DespawnMonsterTotal();
            isSpawning = false;
        }
    }

    private void OnEnable()
    {
        isSpawning = false;
        // MonsterManager.instance.AddMonsterSpawnerSC(sectorVec, this);
    }

    private void OnDisable()
    {
        isSpawning = false;
        // MonsterManager.instance.RemoveMonsterSpawnerSC(sectorVec);
    }
}