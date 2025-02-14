using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    
    Animator animator;
    public float timer;
    void Start()
    {
        TryGetComponent(out animator);
        animator.SetTrigger("StartStage");
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Victory")){
            Debug.Log("현재 애니메이션은 : StartStage...");
            timer = animator.GetCurrentAnimatorStateInfo(0).length;
        }
        Invoke("Func", timer);
    }

    void Func(){
        Debug.Log("Animation TEST!");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
