using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConroller : MonoBehaviour
{
    public Transform target;        
    private Vector3 defaultPos = new Vector3(0, 1.8f, -5f);        
    public float rotationSpeed = 100f;          

    private float currentX = 0f;                
    private float currentY = 0f;               
    private const float Y_ANGLE_MIN = -20.0f;     
    private const float Y_ANGLE_MAX = 50.0f;      

    public float zoomSpeed = 5f;                
    public float minZoom;                
    public float maxZoom;               
    private float currentZoom;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = true;
    }

    void LateUpdate()
    {
        CameraMove();
        CameraZoom();
        SetCamera();
    }

    void CameraMove()
    {
        if (Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            currentX += Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            currentY = Mathf.Clamp(currentY, Y_ANGLE_MIN, Y_ANGLE_MAX);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void CameraZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        currentZoom = Mathf.Clamp(currentZoom + scrollInput * zoomSpeed, -maxZoom, -minZoom);
    }

    void SetCamera()
    {
        Vector3 direction = new Vector3(0, -currentZoom * 0.3f, currentZoom);
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        transform.position = target.position + new Vector3(0, defaultPos.y, 0) + rotation * direction;
        transform.LookAt(target.position + (Vector3.up * 2.5f));
    }
}