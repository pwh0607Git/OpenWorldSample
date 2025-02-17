using System;
using UnityEngine;

//boss controller
public abstract class Boss : MonoBehaviour
{
    protected Animator animator;
    protected CharacterController controller;
    
    protected Vector3 originalPosition;
    [SerializeField] protected bool isPerformingStage;
    [SerializeField] protected bool isDown;
    [SerializeField] protected bool isAttacking;
    float startStageTime;
    public void StartBossStage(){
        animator.SetTrigger("StartStage");
    }

    public void EndBossStage(){
        isPerformingStage = false;
        ReturnOriginalPosition();
    }
    
    public void StartBossAI(){
        isPerformingStage = true;
    }

    void ReturnOriginalPosition(){
        controller.Move(originalPosition * 10f * Time.deltaTime);
    }


    #region TakeDamage
    private event Action<float> OnHpChanged;
    int maxHP = 100;
    int currentHP = 100;
    public void SubscribeToHpChanged(Action<float> callback) => OnHpChanged += callback;
    public void UnsubscribeFromHpChanged(Action<float> callback) => OnHpChanged -= callback;
    public void TakeDamage(int damage){
        currentHP -= damage;
        float percent = currentHP / (float) maxHP; 
        OnHpChanged?.Invoke(percent);
    }
    #endregion

    #region Down
    private event Action OnBossDown;
    public void SubscribeToBossDown(Action callback) => OnBossDown += callback;
    public void UnsubscribeFromBossDown(Action callback) => OnBossDown -= callback;

    protected void Down(){
        animator.SetTrigger("Down");
        OnBossDown?.Invoke();
        Invoke("DestroyBoss",1.5f);
    }

    protected void DestroyBoss(){
        Destroy(this.gameObject);
    }
    #endregion
}