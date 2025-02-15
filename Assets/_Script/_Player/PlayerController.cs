using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerAnimState
{
    Idle, Walk, Jump, Attack1, Attack2, Damaged, Down
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public static PlayerController player { get; private set; }
    
    public static PlayerUIController uiController;
    private static PlayerAttack playerAttack;
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
        uiController = FindAnyObjectByType<PlayerUIController>();
        playerAttack = GetComponentInChildren<PlayerAttack>();
    }

    private Animator animator;
    private PlayerAnimState currentAnimState;

    Transform CamDir;
    public Transform SpawnPos;

    public float moveSpeed;
    private CharacterController controller;
    
    private RaycastHit hit;
    public float maxSlopeAngle = 45f;
    public float gravity = 30f;
    private Vector3 moveDirection;

    void Start()
    {
        InitPlayer();
    }

    void InitPlayer()
    {
        // myState = new State();
        
        controller = GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>(); 
        currentAnimState = PlayerAnimState.Idle;
        moveSpeed = myState.speed;

        //transform.position = SpawnPos.position;
    }

    void Update()
    {
        Move();
        AttackHandler();
        UpdateAnim();
    }

    void Move()
    {
        if (isDamaging || isAttacking) return;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = Camera.main.transform.forward.FlattenY();
        Vector3 cameraRight = Camera.main.transform.right.FlattenY();

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

    [SerializeField] float attackForce = 5.0f;
    [SerializeField] List<GameObject> attackAbleMonsters;

    void AttackHandler()
    {
        //if (weaponTransform.childCount == 0) return;            //무기 미장착시 무시하기.
        UpdateAttackableMonsterList();
        if (isAttacking)
        {
            LookTarget(GetAttackTarget());
            Vector3 attackTargetDirection = GetAttackTarget() == null ? transform.forward : (GetAttackTarget().position - transform.position).normalized;
            controller.Move(attackTargetDirection * attackForce * Time.deltaTime);
        }
        
        if (!Input.GetKeyDown(KeyCode.LeftControl) && !Input.GetKeyDown(KeyCode.RightControl)) return;
        animator.SetTrigger("ComboAttack");
    }

    Transform GetAttackTarget()
    {
        Transform attackTarget = attackAbleMonsters.Count <= 0 ? null : attackAbleMonsters[0].transform;
        return attackTarget;
    }
    private void UpdateAttackableMonsterList()
    {
        attackAbleMonsters = playerAttack.GetAttackableMonsterList();
    }
    void LookTarget(Transform target){
        if(target == null) return;

        Quaternion targetRotation = Quaternion.LookRotation((target.position - transform.position).normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            Destroy(other.gameObject);
            uiController.GetItem(other.gameObject);          //Subscribe 패턴으로 수정.
        }
    }
    [SerializeField] bool isAttacking = false;

    public void OnAttackHit1()
    {   
        isAttacking = true;
        
        foreach (GameObject monster in attackAbleMonsters)
        {
            MonsterControllerBT monsterScript = monster.GetComponent<MonsterControllerBT>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage((int)myState.attack);
            }
        }
    }

    public void OnAttackHit2()
    {
        isAttacking = true;
        
        foreach (GameObject monster in attackAbleMonsters)
        {
            MonsterControllerBT monsterScript = monster.GetComponent<MonsterControllerBT>();
            if (monsterScript != null)
            {
                monsterScript.TakeDamage((int)myState.attack);
            }
        }
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    private bool isDamaging = false;
    [SerializeField] float noDamagingTime = 0.3f;            //0.초간 데미지 받지 않기...

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
    }

    public void CleanEquipment()
    {
        Destroy(weaponTransform.GetChild(0).gameObject);
    }

    //넉백 데이터 추가
    public void ApplyKnockBack(Vector3 directionAndPower){
        controller.Move(directionAndPower * Time.deltaTime);
    }
    //UI Text Method
    public void StartBossStage(){
        uiController.ShowBossUI();
    }
    public void EndBossStage(){
        uiController.CloseBossUI();
    }
    
    public void OnHandleBossStage(){
        
    }
}