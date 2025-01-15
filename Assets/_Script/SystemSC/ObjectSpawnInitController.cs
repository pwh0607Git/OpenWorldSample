using UnityEngine;

public class ObjectSpawnInitController : MonoBehaviour
{
    public Vector3 originalPosition { get; set; }

    public void SetOntheFloor(){
        Ray upRay = new Ray(transform.position + Vector3.down * 1f, Vector3.up);
        Ray downRay = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        RaycastHit hit;
        
        //바닥이 위에 있다면... 바닥 면에 맞닿아야한다.
        if(Physics.Raycast(upRay, out hit, Mathf.Infinity, LayerMask.GetMask("Level")) || Physics.Raycast(downRay, out hit, Mathf.Infinity, LayerMask.GetMask("Level"))){
            
            if(hit.transform.CompareTag("Floor") || hit.transform.GetComponent<Terrain>() != null){
                originalPosition = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
        }
    }
}