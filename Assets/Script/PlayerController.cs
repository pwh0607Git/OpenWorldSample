using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    Transform CamDir;
    public Transform SpawnPos;

    public float moveSpeed = 5f;
    private CharacterController controller;
    private Rigidbody rigidbody;
    
    private RaycastHit hit;

    public float maxSlopeAngle = 45f;
    public float gravity = 30f;
    //public float slopeGravity = 50f;
    private Vector3 moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        rigidbody = GetComponent<Rigidbody>();
        transform.position = SpawnPos.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        Vector3 movement = cameraRight * moveHorizontal + cameraForward * moveVertical;
        movement = Vector3.ClampMagnitude(movement, 1);

        bool isOnSlope = checkSlope();
        Vector3 adjustedMovement = isOnSlope ? AdjustDirectionToSlope(movement) : movement;

        // 중력 처리
        if (controller.isGrounded)
        {
            moveDirection.y = -1;
            if (!isOnSlope) 
            {
                moveDirection.y -= gravity * Time.deltaTime;
            }
        }
        else
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // 이동 방향 합성
        moveDirection.x = adjustedMovement.x;
        moveDirection.z = adjustedMovement.z;

        Debug.Log("Direction Adjust Result : " + moveDirection + "...");

        // 캐릭터 이동
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    bool checkSlope()
    {
        if (controller.isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, controller.height / 2 * 1.1f))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > 0 && angle <= maxSlopeAngle;
            }
        }
        return false;
    }

    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, hit.normal).normalized;
    }
}