using System.Collections;
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
            StartCoroutine(SpawnMonster(data, transX));
        }
    }

    IEnumerator SpawnMonster(MonsterData data, float transX){
        yield return null;

        GameObject monster = Instantiate(data.monsterPrefab);
        MonsterControllerBT monsterController = monster.GetComponent<MonsterControllerBT>();
        monster.transform.position = new Vector3(gameObject.transform.position.x + transX, 5f, gameObject.transform.position.z);
        
        transX += 10f;
        
        if (monsterController != null)
        {
            MonsterBlackBoard blackBoard = new MonsterBlackBoard(data);
            monsterController.InitMonsterBlackBoard(blackBoard);
        }
        SetMonsterStateUI(monster);
    }

    public void SetMonsterStateUI(GameObject monster)
    {
        Instantiate(monsterStateUIPrefab, monster.transform);
    }
}