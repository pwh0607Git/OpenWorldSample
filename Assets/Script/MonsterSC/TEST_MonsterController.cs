using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TEST_MonsterController : MonoBehaviour
{
    MonsterData monsterData;
    public MonsterData MonsterData { get { return monsterData; } set { monsterData = value; } }
    private MonsterStateUIController monsterUI;
    private Animator animator;

    public int monsterCurHP;

    private bool canTakeDamage = true;
    private bool isDown = false;

    Transform attackTarget;
    private float lastAttackTime = -Mathf.Infinity;
    private Vector3 originalPosition;

    private void Start()
    {
        monsterCurHP = monsterData.HP;
        animator = transform.GetComponent<Animator>();
        originalPosition = transform.position;
    }

    private void Update()
    {
        float distanceToTarget = Vector3.Distance(transform.position, attackTarget.position);
        DetectPlayer(distanceToTarget);

        if (IsAttackTarget(distanceToTarget))
        {
            AttackHandler();
        }

        if (monsterCurHP <= 0 && !isDown)
        {
            Down();
        }
    }
    
    private void DetectPlayer(float distanceToTarget)
    {
        if(distanceToTarget < monsterData.detectionRadius)
        {
            ChasePlayer(distanceToTarget);
        }
        else
        {
            ReturnOriginPosition();
        }

    }

    private void ReturnOriginPosition()
    {
        MoveToward(originalPosition);
    }

    private void ChasePlayer(float distanceToTarget)
    {
        MoveToward(attackTarget.position);
    }

    private void MoveToward(Vector3 destination)
    {
        float returnSpeed = 10f;

        Vector3 direction = (destination - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * returnSpeed);
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, returnSpeed * Time.deltaTime);
    }

    private bool IsAttackTarget(float distanceToTarget)
    {
        //현재 타겟이 공격 가능 범위에 들어 왔는가.
        return(distanceToTarget <= monsterData.attackDamageRadius);
    }

    //UI 세팅
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
        Debug.Log($"{monsterData.monsterName}가 공격을 수행합니다!");
        animator.SetTrigger("Attack");

        //애니메이션이 끝나는 시점에 캐릭터가 공격 범위에 있으면 데미지 주기.

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