using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    [System.Serializable]
    public class Monster
    {
        public string name;
        public int HP;
        public int damage;
        public float speed;
        public int size;
        public GameObject prefab;
        public int count;

        public Monster(int HP, int size, int damage, float speed, GameObject prefab, int count)
        {
            this.name = prefab.tag;
            this.HP = HP;
            this.damage = damage;
            this.speed = speed;
            this.size = size;
            this.prefab = prefab;
            this.count = count;
        }
    }

    //차후 사용예정.
    public class Boss : Monster
    {
        public Boss(int HP, int size, int damage, float speed, GameObject prefab, int count) : base(size, HP, damage, speed, prefab, count) { }
    }

    public List<Monster> monsterPrefabs = new List<Monster>();                        //inspector에서 프리팹 추가하기.
    private Dictionary<string, Queue<GameObject>> monsterDictionary;            //monsterTag , queue

    private void Awake()
    {
        monsterDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Monster mon in monsterPrefabs)
        {
            Queue<GameObject> pool = new Queue<GameObject>();

            for (int i = 0; i < mon.count; i++)
            {
                GameObject instance = Instantiate(mon.prefab);
                instance.tag = mon.name; // tag로 구분
                instance.SetActive(false);
                pool.Enqueue(instance);
            }
            monsterDictionary.Add(mon.name, pool);
        }
    }

    public GameObject GetObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (monsterDictionary.ContainsKey(tag) && monsterDictionary[tag].Count > 0)
        {
            GameObject instance = monsterDictionary[tag].Dequeue();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            return instance;
        }
        Debug.LogWarning($"No objects available in pool for tag: {tag}");
        return null;
    }

    public void ReturnObject(GameObject monster)
    {
        monster.SetActive(false);
        string tag = monster.tag;
        monsterDictionary[tag].Enqueue(monster);
    }
}