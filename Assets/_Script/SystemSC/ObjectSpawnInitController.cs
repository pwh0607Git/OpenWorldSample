using UnityEngine;
public class ObjectSpawnInitController : MonoBehaviour
{
    private Vector3 originalPosition; 
    void Start(){
        SetOntheFloor();
    }

    void SetOntheFloor(){
        Ray upRay = new Ray(transform.position, Vector3.up);
        Ray downRay = new Ray(transform.position, Vector3.down);
        RaycastHit hit;

        //바닥이 위에 있다면... 바닥 면에 맞닿아야한다.
        if(Physics.Raycast(upRay, out hit, Mathf.Infinity, LayerMask.GetMask("Level")) || Physics.Raycast(downRay, out hit, Mathf.Infinity, LayerMask.GetMask("Level"))){
            if(hit.transform.CompareTag("Floor")){
                Vector3 newSpawnPos = new Vector3(transform.position.x, hit.transform.position.y, transform.position.z);
                transform.position = newSpawnPos;
                originalPosition = newSpawnPos;
            }
        }
    }

    public Vector3 GetOriginalPosition(){
        Debug.Log($"originalposition : {originalPosition}");
        return originalPosition;
    }
}
