using System.Collections;
using UnityEngine;

public enum PlayerAnimState
{
    Idle, Walk, Jump, Attack1, Attack2, Damaged, Down
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
        TryGetComponent(out controller);
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Item")
        {
            Destroy(other.gameObject);
            myInventory.GetItem(other.gameObject);
        }
    }
    public float attackRange = 1.5f;      
    public bool isAttacking = false;

    private void CheckAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (!isAttacking)
        {
            if (stateInfo.IsName("Attack1"))
            {
                OnAttackHit1();
            }

            if (stateInfo.IsName("Attack2"))
            {
                OnAttackHit2();
            }
            StartCoroutine(Coroutine_PlayerAttack());
        }
    }

    IEnumerator Coroutine_PlayerAttack()
    {
        float fixedTime = 0.5f;
        yield return new WaitForSeconds(fixedTime);

        isAttacking = false;
    }

    public void OnAttackHit1()
    {
        PlayerAttack playerAttack = GetComponentInChildren<PlayerAttack>();
        isAttacking = true;
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
        //StopCoroutine("Coroutine_PlayerAttack");
        PlayerAttack playerAttack = GetComponentInChildren<PlayerAttack>();

        isAttacking = true;
        foreach (GameObject monster in playerAttack.getAttackableMonster)
        {
            TEST_MonsterController monsterScript = monster.GetComponent<TEST_MonsterController>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage((int)myState.attack);
            }
        }
    }
    private bool isDamaging = false;
    private float noDamagingTime = 0.2f;            //0.초간 데미지 받지 않기...
    public void PlayerTakeDamage(int damage){
        if(isDamaging) return;

        myState.curHP -= damage;
        animator.SetTrigger("Damaged");
        myState.NotifyStateChange();
        StartCoroutine(Coroutine_NoDamage());
    }

    IEnumerator Coroutine_NoDamage(){
        isDamaging = true;
        yield return new WaitForSeconds(noDamagingTime);
        isDamaging = false; 
    }

    public Transform weaponTransform;
    
    public void SetEquipment(GameObject item)
    {
        if(weaponTransform.childCount > 0)
        {
            Destroy(weaponTransform.GetChild(0).gameObject);
        }

        GameObject newItem = Instantiate(item, weaponTransform);

        Destroy(newItem.GetComponent<Equipment_DroppedItemSC>());
        Destroy(newItem.GetComponent<Collider>());
    }

    public void CleanEquipment()
    {
        Destroy(weaponTransform.GetChild(0).gameObject);
    }
}