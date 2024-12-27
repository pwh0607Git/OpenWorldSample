using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }

    private Animator animator;

    private MonsterStateUIController monsterUI;

    private bool canTakeDamage = true;
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
        if (monsterCurHP <= 0 && !isDown)
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
        if (isDown || !canTakeDamage)
        {
            Debug.Log("현재는 골렘이 데미지를 받지 않습니다.");
            return;
        }
        StartCoroutine(HandleDamage(damage));
    }

    float noDamage = 0.4f;

    IEnumerator HandleDamage(int damage)
    {
        canTakeDamage = false;

        animator.SetTrigger("Damaged");
        monsterCurHP -= damage;
        monsterUI.UpdateMonsterUI(monsterCurHP);
        Debug.Log($"현재 받은 데미지 : {damage}, 남은 HP : {monsterCurHP}");

        yield return new WaitForSeconds(noDamage);
        canTakeDamage = true;
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

        //이후 몬스터의 애니메이션 명 조정.
        animator.Play("SA_Golem_Down");
        AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);

        Invoke("OnDownMonster", 1.5f);
        
        //전리품 출력.
        MonsterLootHandler loots = transform.GetComponentInChildren<MonsterLootHandler>();

        if(loots != null)
        {
            loots.ShootLoots();
        }
        else
        {
            Debug.Log("전리품 시스템 XXXX");
        }
    }

    public void OnDownMonster()
    {
        Destroy(gameObject);
    }


    //캐릭터 인식 범위
    //공격 범위는 따로 설정하기.
}