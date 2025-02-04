using UnityEngine;
using System;
using System.Collections;

public class MonsterHealth : MonoBehaviour
{
    public event Action<int> OnHPChanged;  // 체력 변경 이벤트
    public event Action OnDeath;
    private MonsterBlackBoard blackboard;
    
    [SerializeField] private float noDamageCooldown = 0.5f;
    private void Start()
    { 
        blackboard = GetComponent<MonsterBlackBoard>();
    }

    public void TakeDamage(int damage)
    {
        if (blackboard.isDamaged) return;      //중복 피격 방지

        blackboard.currentHP -= damage;
        
        if (blackboard.currentHP <= 0)
        {
            OnDeath?.Invoke();
        }
        
        blackboard.isDamaged = true;
        OnHPChanged?.Invoke(blackboard.currentHP);
        StartCoroutine(Coroutine_ResetDamageState());
    }
    IEnumerator Coroutine_ResetDamageState()
    {  
        yield return new WaitForSeconds(noDamageCooldown);
        blackboard.isDamaged = false;
    }
}