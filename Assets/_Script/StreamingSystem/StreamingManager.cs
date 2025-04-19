using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StreamingManager : MonoBehaviour
{
    public Transform player; // �÷��̾� ĳ������ Transform
    public int sectorSize = 10; // �� ������ ũ�� (����: ����)
    public int loadDistance = 1; // �÷��̾�� �� �� ���� �̳��� �Ÿ����� �ε����� ����

    private Dictionary<Vector2Int, string> loadedSectors = new Dictionary<Vector2Int, string>();

    void Update()
    {
        Vector2Int currentSector = GetSector(player.position);

        // �ֺ� ���� �ε�
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
        // �ָ� �ִ� ���� ��ε�
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
        Debug.Log(loadedSectors.Count + " ���� �ε��� �� ����...");
        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        loadedSectors.TryAdd(sector, sceneName);
    }
    */

    private HashSet<Vector2Int> loadingSectors = new HashSet<Vector2Int>(); // �ε� ���� ���� ����

    IEnumerator LoadSector(Vector2Int sector)
    {
        // �ߺ� �ε� ����: �̹� �ε� ���� �������� Ȯ��
        if (loadingSectors.Contains(sector) || loadedSectors.ContainsKey(sector))
        {
            yield break; // �̹� �ε� ���̰ų� �ε� �Ϸ�� ���Ͷ�� ����
        }

        loadingSectors.Add(sector); // �ε� ���� ���ͷ� �߰�\

        string sceneName = $"Sector_{sector.x}_{sector.y}";
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // �ε尡 �Ϸ�Ǹ� �ε�� ���Ϳ� �߰��ϰ� �ε� ���� ���Ϳ��� ����
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