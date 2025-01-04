using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private List<GameObject> attackableMonsters = new List<GameObject>();

    //위 List는 받아서 사용하기만 가능!!
    public List<GameObject> getAttackableMonster { get { return attackableMonsters; } }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsters.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackableMonsters.Remove(other.gameObject);
        }
    }
}
