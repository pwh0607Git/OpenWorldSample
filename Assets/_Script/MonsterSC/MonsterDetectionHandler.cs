using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MonsterDetectionHandler : MonoBehaviour
{
    TEST_MonsterController monsterController;

    private SphereCollider detectionCollider;

    private void Start()
    {
        monsterController = transform.parent.GetComponent<TEST_MonsterController>();
        
        detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = monsterController.MonsterData.detectionRadius;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.SetAttackTarget(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            monsterController.SetAttackTarget(null);
        }
    }
}
