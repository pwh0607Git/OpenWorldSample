using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    private MonsterStateUIController monsterUI;

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log($"{gameObject.name} Damaged : {damage}");
        monsterUI.TakeDamage(damage);
    }
}