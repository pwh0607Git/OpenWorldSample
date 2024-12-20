using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { set { monsterData = value; } }

    private Animator animator;

    private MonsterStateUIController monsterUI;

    private bool isDamaged = true;
    private bool isDown = false;

    public int monsterCurHP;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        AttackHandler();
    }

    private void Update()
    {
        if(monsterData.HP <= 0 && isDown)
        {
            Down();
        }
    }

    public void SetMonsterUI(MonsterStateUIController monsterUI)
    {
        this.monsterUI = monsterUI;
    }

    public void TakeDamage(int damage)
    {
        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);
        Debug.Log($"현재 받은 데미지 : {damage}, 남은 HP : {monsterCurHP}");
    }

    public void AttackHandler()
    {
        Debug.Log("몬스터 공격!!");
        animator.SetTrigger("Attack");
    }

    public void Down()
    {
        Debug.Log("몬스터 Down!!");
        isDown = true;
        animator.SetTrigger("Down");

        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        // 애니메이션 이름이 일치하고, 재생이 끝났는지 확인
        if(currentState.IsName("SA_Golem_Down") && currentState.normalizedTime >= 1.0f)
        {
            Invoke("OnDownMonster", 2f);
        }
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }

    //캐릭터 인식 범위
    //공격 범위는 따로 설정하기.
}