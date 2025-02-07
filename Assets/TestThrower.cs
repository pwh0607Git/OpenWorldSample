using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestThrower : MonoBehaviour
{
    public GameObject prefab;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) MakeObject();
    }

    void MakeObject(){
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
