using System;
using UnityEngine;

public class BasicBossState{
    public int HP;
    public float moveSpeed;
    public BasicBossState(int HP, float moveSpeed){
        this.HP = HP;
        this.moveSpeed = moveSpeed;
    }
}

//boss controller
public abstract class Boss : MonoBehaviour
{
    public static Action OnBossDown;
    protected Animator animator;
    protected CharacterController controller;

    #region Down
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