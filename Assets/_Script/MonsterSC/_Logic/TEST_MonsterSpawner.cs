using System.Collections;
using System.Collections.Generic;
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
        foreach (var data in monsterDatas)
        {
            StartCoroutine(SpawnMonster(data, transX));
        }
    }

    IEnumerator SpawnMonster(MonsterData data, float transX){
        yield return null;

        GameObject monster = Instantiate(data.monsterPrefab);
        monster.transform.position = new Vector3(gameObject.transform.position.x + transX, 5f, gameObject.transform.position.z);
        
        MonsterControllerBT monsterController = monster.GetComponent<MonsterControllerBT>();
        
        transX += 10f;
        if (monsterController != null){
            MonsterBlackBoard blackBoard = monster.AddComponent<MonsterBlackBoard>();
            blackBoard.monsterData = data;
            monsterController.InitMonsterBlackBoard(blackBoard);
            SetMonsterStateUI(monster);
        }
    }

    void SetMonsterStateUI(GameObject monster)
    {
        Instantiate(monsterStateUIPrefab, monster.transform);
    }
}