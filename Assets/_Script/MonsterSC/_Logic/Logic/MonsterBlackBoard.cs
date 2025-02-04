using UnityEngine;

public class MonsterBlackBoard : MonoBehaviour
{
    public MonsterData monsterData;
    public Animator animator;
    public CharacterController controller;
    public Transform player;
    public int currentHP;
    public bool isAttacking;
    public bool isDamaged;
    
    private void Awake()
    {
        monsterData = GetComponent<MonsterData>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        player = GameObject.FindWithTag("Player")?.transform;
        currentHP = monsterData.HP;
        isAttacking = false;
        isDamaged = false;
    }
}