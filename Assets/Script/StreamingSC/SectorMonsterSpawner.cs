using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SectorMonsterSpawner : MonoBehaviour
{
    private ObjectPooling ObjectPooling;
    private List<GameObject> spawnedMonsters = new List<GameObject>();
    
    private bool isSpawning;
    public Vector2Int sectorVec;
    
    void Start()
    {
        ObjectPooling = GetComponent<ObjectPooling>();
        isSpawning = false;
    }

    public void OnPlayerEnter()
    {
        if (!isSpawning)
        {
            SpawnMonster();
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

    public void SpawnMonster()
    {
        Vector3 spawnPosition = gameObject.transform.position;
        GameObject monster = ObjectPooling.GetObject(spawnPosition, Quaternion.identity);
        if (monster != null)
        {
            spawnedMonsters.Add(monster);
        }
    }

    public void DespawnMonsterTotal()
    {
        foreach (GameObject monster in spawnedMonsters)
        {
            ObjectPooling.ReturnObject(monster);
        }
        spawnedMonsters.Clear();
    }
}