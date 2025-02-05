using UnityEngine;

public class MonsterBlackBoard : MonoBehaviour
{
    [Header("Monster Props")]
    public MonsterData monsterData;
    public Animator animator;
    public CharacterController controller;
    public Transform player;

    [Header("Wandor")]
    [SerializeField] public Vector3 originalPosition;
    [SerializeField] public Vector3 nextDestination;

    public int currentHP;
    public bool isMonsterAttacking;
    public bool isMonsterAttackCoolDown;
    public bool isMonsterDamaged;
    
    public MonsterBlackBoard(MonsterData monster){
        this.monsterData = monster;
    }

    [SerializeField] float speedWeight;
    [SerializeField] float rotationSpeed;
    private void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player")?.transform;
        currentHP = monsterData.HP;
        isMonsterAttacking = false;
        isMonsterDamaged = false;
        isMonsterAttackCoolDown = false;

        speedWeight = 3.0f;
        rotationSpeed = 10.0f;
    }
    
    public void MoveToward(Vector3 destination)
    {
        Vector3 moveDirection = ((destination - transform.position).normalized).FlattenY();
        float fixedSpeed = (player == null) ? monsterData.moveSpeed : monsterData.moveSpeed * speedWeight;

        if (controller.isGrounded)
        {
            animator.SetBool("Walk", true);
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
        else
        {
            moveDirection.y += Physics.gravity.y * Time.deltaTime;
        }
        controller.Move(moveDirection * fixedSpeed * Time.deltaTime);
    }
}