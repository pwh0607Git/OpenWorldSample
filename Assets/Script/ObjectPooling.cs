using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPooling : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> monsterPoolDictionary = new Dictionary<string, Queue<GameObject>>();
    public Transform monsters;

    public void InitializeMonsterPool(List<Monster> monsterList)
    {
        foreach (Monster monster in monsterList)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < monster.poolCount; i++)
            {
                GameObject instance = Instantiate(monster.prefab);
                instance.name = monster.prefab.tag;
                instance.transform.SetParent(monsters);
                instance.SetActive(false);
                pool.Enqueue(instance);
            }
            monsterPoolDictionary[monster.prefab.tag] = pool;
        }
    }

    public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (monsterPoolDictionary.ContainsKey(tag) && monsterPoolDictionary[tag].Count > 0)
        {
            GameObject instance = monsterPoolDictionary[tag].Dequeue();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            return instance;
        }
        return null;
    }

    public void ReturnObject(GameObject monster)
    {
        monster.SetActive(false);
        string tag = monster.tag;
        monsterPoolDictionary[tag].Enqueue(monster);
    }
}