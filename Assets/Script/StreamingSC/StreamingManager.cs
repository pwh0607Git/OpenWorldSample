using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StreamingManager : MonoBehaviour
{

    //미터 단위 ... 10m 는 좌표상 100이다. 10배...
    public Transform player; // 플레이어 캐릭터의 Transform
    public int sectorSize = 10; // 각 섹터의 크기 (단위: 미터)
    public int loadDistance = 1; // 플레이어와 몇 개 섹터 이내의 거리에서 로드할지 설정
    
    private Dictionary<Vector2Int, string> loadedSectors = new Dictionary<Vector2Int, string>();

    void Update()
    {
        Vector2Int currentSector = GetSector(player.position);
        Debug.Log($"Player Position: {player.position}, Current Sector: {currentSector}");
        // 주변 섹터 로드
        for (int x = -loadDistance; x <= loadDistance; x++)
        {
            for (int y = -loadDistance; y <= loadDistance; y++)
            {
                Vector2Int sectorToLoad = currentSector + new Vector2Int(x, y);

                //해당 섹터가 로드 되어있지 않다면 로드하기.
                if (!loadedSectors.ContainsKey(sectorToLoad))
                {
                    //Debug.Log($"Loading Sector: {sectorToLoad}");
                    //StartCoroutine(LoadSector(sectorToLoad));
                }
            }
        }

        // 멀리 있는 섹터 언로드
        List<Vector2Int> sectorsToUnload = new List<Vector2Int>();
        foreach (var sector in loadedSectors.Keys)
        {
            if (Vector2Int.Distance(sector, currentSector) > loadDistance)
            {
               // sectorsToUnload.Add(sector);
            }
        }
        foreach (var sector in sectorsToUnload)
        {
           // StartCoroutine(UnloadSector(sector));
        }
    }

    Vector2Int GetSector(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / sectorSize);
        int y = Mathf.FloorToInt(position.z / sectorSize);

        return new Vector2Int(x, y);
    }

    IEnumerator LoadSector(Vector2Int sector)
    {
        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        loadedSectors.Add(sector, sceneName);
    }

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
}
