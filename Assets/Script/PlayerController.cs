using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerAnimState
{
    Idle, Walk, Jump, Attack1, Attack2, Damaged, Down
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private PlayerAnimState currentAnimState;

    Transform CamDir;
    public Transform SpawnPos;

    public float moveSpeed = 5f;
    private CharacterController controller;
    
    private RaycastHit hit;
    public float maxSlopeAngle = 45f;
    public float gravity = 30f;
    private Vector3 moveDirection;

    //Ground Checking 
    /*
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    bool isGrounded;
    */

    void Start()
    {
       // isGrounded = false;
        controller = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        //transform.position = SpawnPos.position;
        currentAnimState = PlayerAnimState.Idle;
    }

    void Update()
    {
        Move();

       // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) Player_Attack();
        UpdateAnim();
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
        movement = Vector3.ClampMagnitude(movement, 1);         //대각선 이동시 값 보정.

        bool isOnSlope = checkSlope();
        Vector3 adjustedMovement = isOnSlope ? AdjustDirectionToSlope(movement) : movement;

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

        moveDirection.x = adjustedMovement.x;
        moveDirection.z = adjustedMovement.z;

        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            currentAnimState = PlayerAnimState.Walk;
        }
        else
        {
            currentAnimState = PlayerAnimState.Idle;
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

    void UpdateAnim()
    {
        if (animator == null)
        {
            Debug.Log("animator is NULL");
            return;
        }

        animator.SetBool("Idle", currentAnimState == PlayerAnimState.Idle);
        animator.SetBool("Walk", currentAnimState == PlayerAnimState.Walk);
        animator.SetBool("Attack1", currentAnimState == PlayerAnimState.Attack1);
    }

    void Player_Attack()
    {
        Debug.Log("Player Attack!!");
        currentAnimState = PlayerAnimState.Attack1;
    }

    void Player_Damaged()
    {
        currentAnimState = PlayerAnimState.Damaged;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            Inventory.myInventory.GetItem(other.gameObject);
        }
    }
}