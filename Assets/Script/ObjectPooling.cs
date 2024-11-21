using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    public GameObject monsterPrefab;
    public int maxCount = 5;

    private Queue<GameObject> monsterPools = new Queue<GameObject>();

    private void Awake()
    {
        for(int i = 0; i < maxCount; i++)
        {
            GameObject monster = Instantiate(monsterPrefab);
            monster.SetActive(false);
            monsterPools.Enqueue(monster);
        }
    }

    public GameObject GetObject(Vector3 position, Quaternion rotation)
    {
        if(monsterPools.Count > 0)
        {
            GameObject monster = monsterPools.Dequeue();
            monster.transform.position = position;
            monster.transform.rotation = rotation;
            monster.SetActive(true);
            return monster;
        }
        return null;
    }

    public void ReturnObject(GameObject monster)
    {
        monster.SetActive(false);
        monsterPools.Enqueue(monster);
    }
}