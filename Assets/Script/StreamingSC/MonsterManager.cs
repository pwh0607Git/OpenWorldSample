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

    private bool isSpawning; // 중복 호출 방지 플래그
    private bool isDespawning;

    private void Start()
    {
        player = GameObject.Find("Player");
        isSpawning = false;
        isDespawning = false;
    }

    private void Update()
    {
        //특정 범위에 들어간 경우 몬스터 Spawn.
        if (!isSpawning)
        {
            StartCoroutine(CallSpawnMonster());
        }

        if (!isDespawning)
        {
            StartCoroutine(CallDespawnMonster());
        }
    }

    //활성화 되어있는 Sector의 몬스터 스폰 오브젝트를 가져온다.
    Dictionary<Vector2Int, SectorMonsterSpawner> monsterSpawners = new Dictionary<Vector2Int, SectorMonsterSpawner>();
    int monsterSpawnCallRange = 1;

    IEnumerator CallSpawnMonster()
    {
        isSpawning = true;
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);
        for (int x = -monsterSpawnCallRange; x <= monsterSpawnCallRange; x++)
        {
            for (int y = -monsterSpawnCallRange; y <= monsterSpawnCallRange; y++)
            {
                Vector2Int spawnCallSector = currentSector + new Vector2Int(x, y);
                while (!monsterSpawners.ContainsKey(spawnCallSector))
                {
                    yield return null;
                }
                monsterSpawners[spawnCallSector].SpawnMonster();
            }
        }
    }

    IEnumerator CallDespawnMonster()
    {
        //캐릭터가 범위를 벗어 났을때..
        Vector2Int currentSector = MapStreamingManager.Instance.GetSector(player.transform.position);

        isDespawning = true;

        //현재 Dictionary에 있는 스포너들의 키(Vector2Int)와 현재 Sector를 비교
        foreach (var sector in monsterSpawners.Keys)
        {
            if(Mathf.Abs(sector.x - currentSector.x) > monsterSpawnCallRange && Mathf.Abs(sector.y - currentSector.y) > monsterSpawnCallRange)
            {
                if (monsterSpawners.ContainsKey(sector))
                {
                    monsterSpawners[sector].DespawnMonster(); // 디스폰 처리
                }
            }
        }

        yield return null;
    }

    public void AddMonsterSpawnerSC(Vector2Int sector, SectorMonsterSpawner spawner)
    {
        if (!monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner 참조 가져오기 성공!");
            monsterSpawners[sector] = spawner;
        }
    }

    public void RemoveMonsterSpawnerSC(Vector2Int sector)
    {
        if (monsterSpawners.ContainsKey(sector))
        {
            Debug.Log($"{sector} Scene spawner 참조 제거하기 성공!");
            monsterSpawners.Remove(sector);
        }
    }
}
