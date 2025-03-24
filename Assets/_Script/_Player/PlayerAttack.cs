using System;
using System.Collections.Generic;
using UnityEngine;




public class PlayerAttack : MonoBehaviour
{
    List<GameObject> attackableMonsterList = new List<GameObject>();

    public List<GameObject> GetAttackableMonsterList() => attackableMonsterList;

    

    #region ���Ͱ� �׾��� �� ����
    private void RemoveMonster(GameObject monster)
    {
        attackableMonsterList.Remove(monster);
    }
    #endregion
}
