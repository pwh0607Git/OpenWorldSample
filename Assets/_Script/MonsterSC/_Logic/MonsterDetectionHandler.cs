using UnityEngine;

public class MonsterDetectionHandler : MonoBehaviour
{
    MonsterController monsterController;

    private SphereCollider detectionCollider;

    public float detectionRadius = 10f;     // ���� �Ÿ�
    public float detectionAngle = 80f;      // ���� ���� (��ä�� ����)

    private void Start()
    {
        monsterController = transform.GetComponentInParent<MonsterController>();
        
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
