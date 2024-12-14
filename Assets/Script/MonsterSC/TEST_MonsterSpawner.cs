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
        //현재 위치에 소환.
        foreach (var data in monsterDatas)
        {
            GameObject monster = Instantiate(data.monsterPrefab, this.transform);
            TEST_MonsterController monsterController = monster.GetComponent<TEST_MonsterController>();

            if (monsterController != null)
            {
                monsterController.MonsterData = data;
            }
            SetMonsterStateUI(monster, data);
        }
    }

    public void SetMonsterStateUI(GameObject monster, MonsterData data)
    {
        GameObject stateUI = Instantiate(monsterStateUIPrefab);
        stateUI.transform.SetParent(monster.transform);

        stateUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data.monsterName;

        Renderer monsterRenderer = monster.transform.GetChild(0).GetComponent<Renderer>();
        if (monsterRenderer != null)
        {
            // 몬스터의 높이 계산
            float monsterHeight = monsterRenderer.bounds.size.y;
            Debug.Log($"{data.monsterName}: {monsterHeight}");

            // UI 위치를 몬스터 머리 위로 이동
            stateUI.transform.localPosition = new Vector3(0, monsterHeight + 0.5f, 0);
        }
    }
}