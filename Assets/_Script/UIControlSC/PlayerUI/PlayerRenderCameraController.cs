using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderCameraController : MonoBehaviour
{
    public GameObject target;
    Vector3 targetHead;

    private void Start()
    {
        float x = target.transform.position.x;
        float newY = target.transform.localPosition.y + 1.1f;
        float z = target.transform.position.z;

        targetHead = new Vector3(x, newY, z);
        transform.LookAt(targetHead);
    }
}
