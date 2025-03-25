using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AttackArea : MonoBehaviour
{
    public HashSet<GameObject> attackableMonsterList = new();

    public UnityAction<HashSet<GameObject>> OnMonsterListChanged;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Monster" || other.tag == "Boss")
            attackableMonsterList.Add(other.gameObject);
        OnMonsterListChanged?.Invoke(attackableMonsterList);
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Monster" || other.tag == "Boss")
            attackableMonsterList.Remove(other.gameObject);
        OnMonsterListChanged?.Invoke(attackableMonsterList);
    }
    
    #region 
    private void RemoveMonster(GameObject monster)
    {
        attackableMonsterList.Remove(monster);
    }
    #endregion
}
