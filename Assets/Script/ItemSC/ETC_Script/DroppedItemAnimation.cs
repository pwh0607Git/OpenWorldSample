using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DroppedItemAnimation : MonoBehaviour
{
    private Collider itemCollider;
    private Vector3 startPosition;

    private float floatAmplitude = 0.12f;     // 위아래 이동 범위
    private float floatFrequency = 1f;       // 이동 속도

    private void Start()
    {
        itemCollider = GetComponent<Collider>();
        if(itemCollider != null)
        {
            float x = transform.position.x;
            float y = itemCollider.bounds.center.y + floatAmplitude;
            float z = transform.position.z;

            startPosition = new Vector3(x, y, z);
        }
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
