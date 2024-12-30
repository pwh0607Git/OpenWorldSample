using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;
using static UnityEngine.CullingGroup;

public enum PlayerAnimState
{
    Idle, Walk, Jump, Attack1, Attack2, Damaged, Down
}

public class State
{
    public int maxHP;
    public int curHP;
    public int maxMP;
    public int curMP;

    public float speed;
    public float defend;
    public float attack;

    public Action OnStateChanged;

    public State()
    {
        maxHP = 100;
        curHP = 100;

        maxMP = 100;
        curMP = 100;
        speed = 10f;
        defend = 50f;
        attack = 500f;
    }

    public void EquipItem(Equipment item)
    {
        Debug.Log($"Equip Item {item.itemName}");
        switch (item.subType)
        {
            case EquipmentType.Head:
                {
                    defend += item.value;
                    break;
                }
            case EquipmentType.Weapon:
                {
                    attack += item.value;
                    break;
                }
            case EquipmentType.Cloth:
                {
                    maxHP += (int)item.value;
                    break;
                }
            case EquipmentType.Foot:
                {
                    speed += item.value;
                    break;
                }
        }
        NotifyStateChange();
    }

    public void DetachItem(Equipment item)
    {
        Debug.Log($"Detach Item {item.itemName}");
        switch (item.subType)
        {
            case EquipmentType.Head:
            {
                defend -= item.value;
                break;
            }
            case EquipmentType.Weapon:
            {
                attack -= item.value;
                break;
            }
            case EquipmentType.Cloth:
            {
                maxHP -= (int)item.value;
                break;
            }
            case EquipmentType.Foot:
            {
                speed -= item.value;
                break;
            }
        }
        NotifyStateChange();
    }

    public void UesConsumable(Consumable itemData)
    {
        switch (itemData.subType)
        {
            case ConsumableType.HP:
                {
                    curHP += (int)itemData.value;
                    if (curHP >= maxHP) curHP = maxHP;
                    break;
                }
            case ConsumableType.MP:
                {
                    curMP += (int)itemData.value;
                    if (curMP >= maxMP) curMP = maxMP;
                    break;
                }
            case ConsumableType.SpeedUp:
                {
                    speed += itemData.value;
                    float duration = 10f;
                    PlayerController.myBuffManager.OnBuffItem(itemData, duration);
                    break;
                }
        }
        NotifyStateChange();
    }

    private void NotifyStateChange()
    {
        OnStateChanged?.Invoke();
    }
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController player { get; private set; }
    public static Inventory myInventory;
    public static EquipmentWindow myEquipments;
    public static ActionBar myKeyboard;
    public static BuffManager myBuffManager;
    public State myState;

    private void Awake()
    {
        if (player == null)
        {
            player = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        InitUI();
    }

    void InitUI()
    {
        myInventory = FindAnyObjectByType<Inventory>();
        myEquipments = FindAnyObjectByType<EquipmentWindow>();
        myKeyboard = FindAnyObjectByType<ActionBar>();
        myBuffManager = FindAnyObjectByType<BuffManager>();
    }

    private Animator animator;
    private PlayerAnimState currentAnimState;

    Transform CamDir;
    public Transform SpawnPos;

    private float moveSpeed;
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
        myState = new State();
        controller = GetComponent<CharacterController>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        currentAnimState = PlayerAnimState.Idle;
        moveSpeed = myState.speed;

        // isGrounded = false;
        //transform.position = SpawnPos.position;
    }

    void Update()
    {
        Move();
        // isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl)) AttackHandler();
        UpdateAnim();
        CheckAttack();
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
    }

    void AttackHandler()
    {
        animator.SetTrigger("ComboAttack");
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
            myInventory.GetItem(other.gameObject);
        }
    }

    //Attack 처리.
    //나중에 무기에 따라 공격 범위 세팅하기.
    public float attackRange = 1.5f;                // 공격 범위
    
    private void CheckAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("Attack1"))
        {
            OnAttackHit1();
        }

        if (stateInfo.IsName("Attack2"))
        {
            OnAttackHit2();

        }
    }

    public void OnAttackHit1()
    {
        PlayerAttack playerAttack = GetComponentInChildren<PlayerAttack>();

        foreach (GameObject monster in playerAttack.getAttackableMonster)
        {
            TEST_MonsterController monsterScript = monster.GetComponent<TEST_MonsterController>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage((int)myState.attack);
            }
        }
    }

    public void OnAttackHit2()
    {
        PlayerAttack playerAttack = GetComponentInChildren<PlayerAttack>();

        foreach (GameObject monster in playerAttack.getAttackableMonster)
        {
            TEST_MonsterController monsterScript = monster.GetComponent<TEST_MonsterController>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage((int)myState.attack);
            }
        }
    }

    //장비처리 코드
    public Transform weaponTransform;
    
    public void SetEquipment(GameObject item)
    {
        if(weaponTransform.childCount > 0)
        {
            Destroy(weaponTransform.GetChild(0).gameObject);
        }

        GameObject newItem = Instantiate(item, weaponTransform);

        Destroy(newItem.GetComponent<DroppedItemAnimation>());                  //이후에는 아이템이 드롭되었을 때 해당 컴포넌트를 추가하는 방식
        Destroy(newItem.GetComponent<Equipment_DroppedItemSC>());
        Destroy(newItem.GetComponent<Collider>());
    }

    public void CleanEquipment()
    {
        Destroy(weaponTransform.GetChild(0).gameObject);
    }
}