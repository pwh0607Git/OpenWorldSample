using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapStreamingManager : MonoBehaviour
{
    public static MapStreamingManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform player;
    public int sectorSize = 200;
    public int loadDistance = 2;
    public Vector2Int currentSector;

    public MonsterManager monsterManager;

    private void Start()
    {
        currentSector = GetSector(player.position);
        monsterManager = MonsterManager.instance;
        initMainScene();
    }

    void initMainScene()
    {
        LoadSectorFunc();
        // UnloadSectorFunc();
    }

    private void Update()
    {
        if(checkChangeSector()){
            LoadSectorFunc();
            UnloadSectorFunc();
            monsterManager.MonsterSpawnCall();
        }
    }

    public Vector2Int maxSector = new Vector2Int(2, 2);
    public Vector2Int minSector = new Vector2Int(-2, -2);

    bool CheckValidSector(Vector2Int sector)
    {
        return (sector.x >= minSector.x && sector.x <= maxSector.x && sector.y >= minSector.y && sector.y <= maxSector.y);
    }

    public static Dictionary<Vector2Int, bool> LoadedSectorState = new Dictionary<Vector2Int, bool>();

    HashSet<Vector2Int> LoadSectorTask = new HashSet<Vector2Int>();

    void LoadSectorFunc()
    {
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int y = -loadDistance; y <= loadDistance; y++)
            {
                Vector2Int sectorToLoad = currentSector + new Vector2Int(x, y);

                if (CheckValidSector(sectorToLoad))
                {
                    if (!LoadedSectorState.ContainsKey(sectorToLoad))
                    {
                        LoadedSectorState.Add(sectorToLoad, false);
                    }

                    if (!LoadedSectorState[sectorToLoad] && !LoadSectorTask.Contains(sectorToLoad))
                    {
                        LoadSectorTask.Add(sectorToLoad);
                        StartCoroutine(LoadSector(sectorToLoad));
                    }
                }
            }
        }
    }

    HashSet<Vector2Int> UnLoadSectorTask = new HashSet<Vector2Int>();

    void UnloadSectorFunc()
    {
        List<Vector2Int> sectorsToUnload = new List<Vector2Int>();

        for (int x = minSector.x; x <= maxSector.x; x++)
        {
            for (int y = minSector.y; y <= maxSector.y; y++)
            {
                Vector2Int sector = new Vector2Int(x, y);

                if (Mathf.Abs(sector.x - currentSector.x) > loadDistance || Mathf.Abs(sector.y - currentSector.y) > loadDistance)
                {
                    sectorsToUnload.Add(sector);
                }
            }
        }

        foreach (var sector in sectorsToUnload)
        {
            if (LoadedSectorState[sector] && !UnLoadSectorTask.Contains(sector))
            {
                UnLoadSectorTask.Add(sector);
                StartCoroutine(UnloadSector(sector));
            }
        }
    }

    IEnumerator LoadSector(Vector2Int sectorToLoad)
    {
        string sceneName = $"Sector_{sectorToLoad.x}_{sectorToLoad.y}";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        asyncLoad.completed += (AsyncOperation op) =>
        {
            LoadSectorTask.Remove(sectorToLoad);
            LoadedSectorState[sectorToLoad] = true;
        };
    }

    IEnumerator UnloadSector(Vector2Int sectorToUnLoad)
    {
        string sceneName = $"Sector_{sectorToUnLoad.x}_{sectorToUnLoad.y}";

        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        asyncUnload.completed += (AsyncOperation op) =>
        {
            UnLoadSectorTask.Remove(sectorToUnLoad);
            LoadedSectorState[sectorToUnLoad] = false;
        };
    }

    public Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x + sectorSize / 2) / sectorSize);
        int y = Mathf.FloorToInt((position.z + sectorSize / 2) / sectorSize);

        return new Vector2Int(x, y);
    }

    public bool checkChangeSector()
    {
        Vector2Int newSector = GetSector(player.position);

        if (currentSector != newSector)
        {
            currentSector = newSector;
            return true;
        }
        return false;
    }
}