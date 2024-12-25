using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_MonsterSpawner : MonoBehaviour
{
    public List<MonsterData> monsterDatas = new List<MonsterData>();
    public GameObject monsterStateUIPrefab;
    
    private void Start()
    {
        SpawnMonsters();
    }

    public void SpawnMonsters()
    {
        float transX = -10f;

        //현재 위치에 소환.
        foreach (var data in monsterDatas)
        {
            GameObject monster = Instantiate(data.monsterPrefab);
            TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();

            monster.transform.position = new Vector3(transX, gameObject.transform.position.y, gameObject.transform.position.z);
            
            transX += 10f;
            
            if (monsterController != null)
            {
                monsterController.MonsterData = data;
            }
            SetMonsterStateUI(monster, data);
        }
    }

    public void SetMonsterStateUI(GameObject monster, MonsterData data)
    {
        GameObject stateUI = Instantiate(monsterStateUIPrefab, monster.transform);
        MonsterStateUIController monsterStateUIController = stateUI.GetComponent<MonsterStateUIController>();
        monsterStateUIController.InitMonsterUI(data);

        Collider monsterCollider = monster.GetComponent<Collider>();
        if (monsterCollider != null)
        {
            float monsterHeight = monsterCollider.bounds.size.y;
            stateUI.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, monsterHeight, 0);
        }

        //controller에 ui참조 적용
        TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();
        monsterController.SetMonsterUI(monsterStateUIController);
    }

    public void SetLoot(GameObject monster)
    {
        for(int i = 0; i < 3; i++)
        {
            GameObject loot = new GameObject("loot " + i);
            loot.transform.SetParent(monster.transform);
        }
    }
}