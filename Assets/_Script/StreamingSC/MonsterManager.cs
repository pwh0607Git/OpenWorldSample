using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    [SerializeField] int monsterSpawnCallRange = 1;

    private void Start()
    {
        MonsterSpawnCall();
    }

    Dictionary<Vector2Int, SectorMonsterSpawner> monsterSpawners = new Dictionary<Vector2Int, SectorMonsterSpawner>();

    public void MonsterSpawnCall()
    {
        StartCoroutine(NotifySpawner());
    }

    IEnumerator NotifySpawner()
    {
        HashSet<Vector2Int> activeSpawners = new HashSet<Vector2Int>();
        Vector2Int currentSector = MapStreamingManager.Instance.currentSector;
        for (int x = -monsterSpawnCallRange; x <= monsterSpawnCallRange; x++)
        {
            for(int y = -monsterSpawnCallRange; y <= monsterSpawnCallRange; y++)
            {
                Vector2Int ranSector = currentSector + new Vector2Int(x, y);
                while (!monsterSpawners.ContainsKey(ranSector))
                {
                    yield return null;
                }
                activeSpawners.Add(ranSector);
            }
        }

        foreach (var sector in activeSpawners)
        {
            if (monsterSpawners.ContainsKey(sector))
            {
                monsterSpawners[sector].OnPlayerEnter();
                yield return null;
            }
        }

        HashSet<Vector2Int> unActiveSpawners = new HashSet<Vector2Int>();

        foreach(var sector in monsterSpawners)
        {
            if (Mathf.Abs(sector.Key.x - currentSector.x) > monsterSpawnCallRange || Mathf.Abs(sector.Key.y - currentSector.y) > monsterSpawnCallRange)
            {
                unActiveSpawners.Add(sector.Key);
            }
        }

        foreach (var sector in unActiveSpawners)
        {
            if (monsterSpawners.ContainsKey(sector))
            { 
                monsterSpawners[sector].OnPlayerExit();
                yield return null;
            }
        }
    }

    public void AddMonsterSpawnerSC(Vector2Int sector, SectorMonsterSpawner spawner)
    {
        if(!monsterSpawners.ContainsKey(sector)) monsterSpawners.Add(sector, spawner);
    }

    public void RemoveMonsterSpawnerSC(Vector2Int sector)
    {
        if (monsterSpawners.ContainsKey(sector)) monsterSpawners.Remove(sector);
    }
}