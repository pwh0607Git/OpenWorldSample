using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private List<GameObject> attackableMonsters = new List<GameObject>();

    public List<GameObject> getAttackableMonster
    {
        get
        {
            attackableMonsters.RemoveAll(monster => monster == null);
            return(attackableMonsters.Count == 0 ? null : attackableMonsters);
        }
    }

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
