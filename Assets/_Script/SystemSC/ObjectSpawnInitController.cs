using UnityEngine;

public class ObjectSpawnInitController : MonoBehaviour
{
    public Vector3 originalPosition { get; set; }
    public bool isSettingComplete = false;
    public void SetOntheFloor(){
        Ray upRay = new Ray(transform.position + Vector3.down * 1f, Vector3.up);
        Ray downRay = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        RaycastHit hit;

        if(Physics.Raycast(upRay, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")) || Physics.Raycast(downRay, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))){
            if(hit.transform.CompareTag("Floor") || hit.transform.GetComponent<Terrain>() != null){
                originalPosition = hit.point;
            }
        }
    }
}