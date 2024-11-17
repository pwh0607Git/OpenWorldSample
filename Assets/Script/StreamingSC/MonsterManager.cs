using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("Player");
    }

    private void Update()
    {
        //특정 범위에 들어간 경우 몬스터 Spawn.
        CallMonsterSpawn();
    }

    //활성화 되어있는 Sector의 몬스터 스폰 오브젝트를 가져온다.
    Dictionary<Vector2Int, SectorMonsterSpawner> monsterSpawners = new Dictionary<Vector2Int, SectorMonsterSpawner>();
    int monsterSpawnCallRange = 1;

    void CallMonsterSpawn()
    {
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);
        for (int x = -monsterSpawnCallRange; x <= monsterSpawnCallRange; x++)
        {
            for (int y = -monsterSpawnCallRange; y <= monsterSpawnCallRange; y++)
            {
                Vector2Int spawnCallSector = currentSector + new Vector2Int(x, y);
                
            }
        }
    }

    //외부 씬에서 Load 되었을때 이 중앙 처리스크립트로 가져오기.
    public void AddMonsterSpawnerSC(Vector2Int sector, SectorMonsterSpawner spawner)
    {
        if (!monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner 참조 가져오기 성공!");
            monsterSpawners[sector] = spawner;
        }
    }

    //UnLoad시..
    public void RemoveMonsterSpawnerSC(Vector2Int sector)
    {
        if (monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner 참조 제거하기 성공!");
            monsterSpawners.Remove(sector);
        }
    }
}
