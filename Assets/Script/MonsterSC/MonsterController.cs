using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using System;
using static UnityEngine.EventSystems.EventTrigger;

public enum MonsterAnimState
{
    Idle, Walk, Attack, Damaged, Down
}

public class MonsterController : MonoBehaviour
{
    private Animator animator;
    public event Action OnMonsterDeath;                 // 몬스터 사망 이벤트

    [SerializeField]
    class State
    {
        public int HP { get; set; }
        public float speed { get; set; }
        public int attack { get; set; }

        public State(int HP, float speed, int attack)
        {
            this.HP = HP;
            this.speed = speed;
            this.attack = attack;
        }
    }

    private State currentMonsterState;
    private MonsterAnimState currentAnimState = MonsterAnimState.Idle;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    void Initialize(int HP, float speed, int attack)
    {
        currentMonsterState = new State(HP, speed, attack);
    }

    private void Update()
    {
        UpdateAnim();
    }

    void CheckState()
    {
        if (currentMonsterState.HP <= 0)
        {
            ChangeState(MonsterAnimState.Down);
            Down();
        }
    }

    public void ChangeState(MonsterAnimState changeState)
    {
        Debug.Log("Changing Anim!!");
        currentAnimState = changeState;
    }

    void UpdateAnim()
    {
        animator.SetBool("Idle", currentAnimState == MonsterAnimState.Idle);
        animator.SetBool("Walk", currentAnimState == MonsterAnimState.Walk);
        //animator.SetBool("Attack", currentAnimState == MonsterAnimState.Attack);
        //animator.SetBool("Damaged", currentAnimState == MonsterAnimState.Damaged);
        //animator.SetBool("Down", currentAnimState == MonsterAnimState.Down);
    }

    void Down()
    {
        Debug.Log($"{gameObject.tag} is Down!");
        OnMonsterDeath?.Invoke();
        gameObject.SetActive(false);
        currentAnimState = MonsterAnimState.Down;
    }

    public float attackRadius = 2f; // 공격 반경
    public float attackDamage = 10f; // 공격 데미지
    public Transform attackPoint; // 공격 중심 위치

    public void PerformAttack()
    {
        // 공격 범위 내의 적 감지
        Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

        foreach (Collider collider in hitColliders)
        {
            // 적인지 확인
            if (collider.CompareTag("Player"))
            {
                // 데미지 처리 (예: 적 스크립트에서 TakeDamage 함수 호출)
                //collider.GetComponent<PlayerController>().TakeDamage(attackDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}