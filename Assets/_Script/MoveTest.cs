using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest : MonoBehaviour
{
    public Vector3 pos;
    public float delta;
    public float speed;
    void Start(){
        pos= transform.position;
    }
    void Update () {
        Vector3 v = pos;
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
}
