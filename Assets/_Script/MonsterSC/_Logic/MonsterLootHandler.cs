using System.Collections.Generic;
using UnityEngine;

public class MonsterLootHandler : MonoBehaviour
{
    MonsterData monsterData;

    List<GameObject> loots;

    private void Start()
    {
        loots = new List<GameObject>();
        monsterData = transform.parent.GetComponent<TEST_MonsterController>().MonsterData;
        
        if(monsterData != null)
        {
            MakeBasicLoot();
            MakeRandomLoot();
        }

        SetLootRef();
        gameObject.SetActive(false);
    }

    void SetLootRef()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                loots.Add(child.gameObject);
            }
        }
    }

    void MakeBasicLoot()
    {
        foreach (var loot in monsterData.basicLoot)
        {
            GameObject instance = SetDroppedProperties(Instantiate(loot, transform));
        }
    }

    void MakeRandomLoot()
    {
        foreach(var loot in monsterData.randomLoot){
            int randomNumber = Random.Range(0, 100);
            if(randomNumber <= loot.dropRate)
            {
                GameObject instance = SetDroppedProperties(Instantiate(loot.item, transform));
            }
        }
    }

    public void ShootLoots()
    {
        foreach (var loot in loots)
        {
            loot.transform.SetParent(null);
        }
    }

    GameObject SetDroppedProperties(GameObject droppedObject)
    {
        droppedObject.AddComponent<Rigidbody>();
        droppedObject.AddComponent<BoxCollider>();

        return droppedObject;
    }
}