using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StreamingManager : MonoBehaviour
{
    public Transform player; // 플레이어 캐릭터의 Transform
    public int sectorSize = 10; // 각 섹터의 크기 (단위: 미터)
    public int loadDistance = 1; // 플레이어와 몇 개 섹터 이내의 거리에서 로드할지 설정

    private Dictionary<Vector2Int, string> loadedSectors = new Dictionary<Vector2Int, string>();

    void Update()
    {
        Vector2Int currentSector = GetSector(player.position);

        // 주변 섹터 로드
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int y = -loadDistance; y <= loadDistance; y++)
            {
                Vector2Int sectorToLoad = currentSector + new Vector2Int(x, y);
                if (!loadedSectors.ContainsKey(sectorToLoad))
                {
                    Debug.Log($"Sector_{sectorToLoad.x}_{sectorToLoad.y} Load...");
                    StartCoroutine(LoadSector(sectorToLoad));
                }
            }
        }

        /*
        // 멀리 있는 섹터 언로드
        List<Vector2Int> sectorsToUnload = new List<Vector2Int>();
        foreach (var sector in loadedSectors.Keys)
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
        */
    }

    Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / sectorSize);
        int y = Mathf.FloorToInt(position.z / sectorSize);
        return new Vector2Int(x, y);
    }
    /*
    IEnumerator LoadSector(Vector2Int sector)
    {
        Debug.Log(loadedSectors.Count + " 현재 로딩된 씬 개수...");
        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadedSectors.TryAdd(sector, sceneName);
    }
    */

    private HashSet<Vector2Int> loadingSectors = new HashSet<Vector2Int>(); // 로드 중인 섹터 관리

    IEnumerator LoadSector(Vector2Int sector)
    {
        // 중복 로드 방지: 이미 로드 중인 섹터인지 확인
        if (loadingSectors.Contains(sector) || loadedSectors.ContainsKey(sector))
        {
            yield break; // 이미 로드 중이거나 로드 완료된 섹터라면 종료
        }

        loadingSectors.Add(sector); // 로드 중인 섹터로 추가\

        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // 로드가 완료되면 로드된 섹터에 추가하고 로드 중인 섹터에서 제거
        loadedSectors[sector] = sceneName;
        loadingSectors.Remove(sector);
    }

    /*
    IEnumerator UnloadSector(Vector2Int sector)
    {
        if (loadedSectors.TryGetValue(sector, out string sceneName))
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
            loadedSectors.Remove(sector);
        }
    }
    */
}