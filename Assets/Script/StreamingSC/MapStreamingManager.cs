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

    public static Dictionary<Vector2Int, bool> LoadedSectorState = new Dictionary<Vector2Int, bool>();

    public Transform player;
    public int sectorSize = 200;
    public int loadDistance = 1;

    private void Update()
    {
        Vector2Int currentSector = GetSector(player.position);
        Debug.Log($"사용자의 현재 Sector : ({currentSector.x}, {currentSector.y})");
        LoadSectorFunc(currentSector);
        UnloadSectorFunc(currentSector);
    }

    Vector2Int maxSector = new Vector2Int(2, 2);
    Vector2Int minSector = new Vector2Int(-2, -2);

    //유효성 검사 메서드
    bool CheckValidSector(Vector2Int sector)
    {
        return (sector.x >= minSector.x && sector.x <= maxSector.x && sector.y >= minSector.y && sector.y <= maxSector.y);
    }

    void LoadSectorFunc(Vector2Int currentSector)
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

                    if (!LoadedSectorState[sectorToLoad])
                    {
                        LoadedSectorState[sectorToLoad] = true;
                        StartCoroutine(LoadSector(sectorToLoad));
                    }
                }
            }
        }
    }

    HashSet<Vector2Int> UnLoadSceneTask = new HashSet<Vector2Int>();

    void UnloadSectorFunc(Vector2Int currentSector)
    {
        foreach (var sector in LoadedSectorState.Keys)
        {
            if (CheckValidSector(sector))
            {
                if (Mathf.Abs(sector.x - currentSector.x) > loadDistance || Mathf.Abs(sector.y - currentSector.y) > loadDistance)
                {
                    if (LoadedSectorState[sector])
                    {
                        LoadedSectorState[sector] = false;
                        StartCoroutine(UnloadSector(sector));
                    }
                }
            }

        }
    }

    public Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x + sectorSize / 2) / sectorSize);
        int y = Mathf.FloorToInt((position.z + sectorSize / 2) / sectorSize);

        return new Vector2Int(x, y);
    }

    IEnumerator LoadSector(Vector2Int sectorToLoad)
    {
        //string sceneName = $"MapSector_{sectorToLoad.x}_{sectorToLoad.y}";
        string sceneName = $"Sector_{sectorToLoad.x}_{sectorToLoad.y}";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        asyncLoad.completed += (AsyncOperation op) =>
        {
            //호출 이벤트.
            //Debug.Log($"Scene '{sceneName}' has been loaded Event");
        };
        LoadedSectorState[sectorToLoad] = true;
    }

    IEnumerator UnloadSector(Vector2Int sectorToUnLoad)
    {
        //string sceneName = $"MapSector_{sectorToUnLoad.x}_{sectorToUnLoad.y}";
        string sceneName = $"Sector_{sectorToUnLoad.x}_{sectorToUnLoad.y}";
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        asyncUnload.completed += (AsyncOperation op) =>
        {
            //Debug.Log($"Scene '{sceneName}' has been Unloaded Event");
        };

        UnLoadSceneTask.Remove(sectorToUnLoad);
        LoadedSectorState[sectorToUnLoad] = false;
    }
}