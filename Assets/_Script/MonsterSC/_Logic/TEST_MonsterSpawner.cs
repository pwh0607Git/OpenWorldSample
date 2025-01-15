using System.Collections.Generic;
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

        foreach (var data in monsterDatas)
        {
            GameObject monster = Instantiate(data.monsterPrefab);
            TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();
            monster.transform.position = new Vector3(gameObject.transform.position.x + transX, 5f, gameObject.transform.position.z);
            
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
        Instantiate(monsterStateUIPrefab, monster.transform);
    }
}