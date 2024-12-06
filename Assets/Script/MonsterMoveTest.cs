using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMoveTest : MonoBehaviour
{
    private LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Ground");
        float rayDistance = 100f;

        Debug.DrawRay(transform.position, Vector3.down * rayDistance, Color.red, 5f);

        // Raycast로 지면과 충돌 검사
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, rayDistance, mask))
        {
            transform.position = hit.point;
        }
    }
}
