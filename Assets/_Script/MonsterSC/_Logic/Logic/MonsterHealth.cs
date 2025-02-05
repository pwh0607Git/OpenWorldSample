using UnityEngine;
using System;
using System.Collections;

public class MonsterHealth : MonoBehaviour
{
    private MonsterBlackBoard blackBoard;
    public event Action<int> OnHPChanged;  // 체력 변경 이벤트
    public event Action OnDeath;
    
    [SerializeField] private float noDamageCooldown = 0.5f;
    private void Start()
    { 
        blackBoard = GetComponent<MonsterBlackBoard>();
    }

    public void TakeDamage(int damage)
    {
        if (blackBoard.isMonsterDamaged) return;      //중복 피격 방지

        blackBoard.currentHP -= damage;
        
        if (blackBoard.currentHP <= 0)
        {
            OnDeath?.Invoke();
        }
        
        blackBoard.isMonsterDamaged = true;
        OnHPChanged?.Invoke(blackBoard.currentHP);
        StartCoroutine(Coroutine_ResetDamageState());
    }


    public void HandleDamageAnim()
    {
        if (blackBoard.isMonsterDamaged && !blackBoard.animator.GetCurrentAnimatorStateInfo(0).IsName("Damaged"))              // 🔥 `isDamaged`가 true이면 애니메이션 실행하도록 수정
        {
            blackBoard.animator.SetTrigger("Damaged");
        }   
    }

    IEnumerator Coroutine_ResetDamageState()
    {  
        yield return new WaitForSeconds(noDamageCooldown);
        blackBoard.isMonsterDamaged = false;
    }
}