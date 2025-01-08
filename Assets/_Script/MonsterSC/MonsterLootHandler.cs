using System.Collections.Generic;
using UnityEngine;

public class MonsterLootHandler : MonoBehaviour
{
    MonsterData monsterData;
    List<LootEntry> lootEntries;

    private void Start()
    {
        monsterData = transform.parent.GetComponent<TEST_MonsterController>().MonsterData;
        
        if(monsterData != null)
        {
            MakeBasicLoot();
            MakeRandomLoot();
        }
    }

    void MakeBasicLoot()
    {
        foreach (var loot in monsterData.basicLoot)
        {
            GameObject lootInstance = Instantiate(loot, transform);
            lootInstance.SetActive(false);
        }
    }

    void MakeRandomLoot()
    {
        foreach(var loot in monsterData.randomLoot){
            int randomNumber = Random.Range(0, 100);
            if(randomNumber <= loot.dropRate)
            {
                GameObject lootInstance = Instantiate(loot.item, transform);
                lootInstance.SetActive(false);
            }
        }
    }

    public void ShootLoots()
    {
        List<GameObject> loots = new List<GameObject>();

        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if(child != null)
            {
                loots.Add(child.gameObject);
            }
        }

        foreach (var loot in loots)
        {
            loot.transform.SetParent(null);
            loot.SetActive(true);
        }
    }
}