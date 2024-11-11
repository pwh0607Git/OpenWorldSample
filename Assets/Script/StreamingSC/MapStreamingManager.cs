using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    //섹터별 스트리밍 상태.
    public static Dictionary<Vector2Int, bool> LoadedSectorState = new Dictionary<Vector2Int, bool>();

    public Transform player;
    public int sectorSize = 100;
    public int loadDistance = 1;

    private void Update()
    {
        Vector2Int currentSector = GetSector(player.position);

        LoadSectorFunc(currentSector);
        UnloadSectorFunc(currentSector);
    }

    void LoadSectorFunc(Vector2Int currentSector)
    {
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int y = -loadDistance; y <= loadDistance; y++)
            {
                Vector2Int sectorToLoad = currentSector + new Vector2Int(x, y);

                if (!LoadedSectorState.ContainsKey(sectorToLoad))
                {
                    LoadedSectorState.Add(sectorToLoad, false);
                }

                if (!LoadedSectorState[sectorToLoad])
                {
                    LoadedSectorState[sectorToLoad] = true;
                    Debug.Log($"Sector_{sectorToLoad.x}_{sectorToLoad.y} Load...");
                    StartCoroutine(LoadSector(sectorToLoad));
                }
            }
        }
    }

    void UnloadSectorFunc(Vector2Int currentSector)
    {
        List<Vector2Int> sectorsToUnload = new List<Vector2Int>();

        foreach (var sector in LoadedSectorState.Keys)
        {
            if (Vector2Int.Distance(sector, currentSector) > loadDistance)
            {
                sectorsToUnload.Add(sector);
            }
        }

        foreach (var sector in sectorsToUnload)
        {
            StartCoroutine(UnloadSector(sector));
        }
    }

    Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / sectorSize);
        int y = Mathf.FloorToInt(position.z / sectorSize);
        return new Vector2Int(x, y);
    }

    IEnumerator LoadSector(Vector2Int sectorToLoad)
    {
        string sceneName = $"Sector_{sectorToLoad.x}_{sectorToLoad.y}";

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        LoadedSectorState[sectorToLoad] = true;
    }

    IEnumerator UnloadSector(Vector2Int sectorToUnLoad)
    {
        string sceneName = $"Sector_{sectorToUnLoad.x}_{sectorToUnLoad.y}";
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        LoadedSectorState[sectorToUnLoad] = false;
    }
}
