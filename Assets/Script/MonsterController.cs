using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using System;

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
        //CheckState();
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
}