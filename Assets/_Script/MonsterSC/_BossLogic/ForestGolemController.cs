using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBossState{
    public int HP;
    public float moveSpeed;
    public BasicBossState(int HP, float moveSpeed){
        this.HP = HP;
        this.moveSpeed = moveSpeed;
    }
}

public class ForestGolemController : MonoBehaviour
{
    public GameObject rockPrefab;
    [SerializeField] GameObject takenRock;  //현재 손에 쥐고 있는 돌.
    public Transform takenRockPosition;
    [SerializeField] GameObject target;
    public Animator animator;
    
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        TryGetComponent(out animator);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            animator.SetTrigger("Long-RangeAttack");
        }
    }

    //animation 이벤트로 출력예정.
    public void StartThrowRock(){
        takenRock = Instantiate(rockPrefab, takenRockPosition);
        Debug.Log("돌 만들기");
    }

    public void ThrowRock(){
        Debug.Log("돌 던지기!");
        takenRock.GetComponent<ThrowAbleStone>().Throw();
        takenRock.transform.SetParent(null);
        takenRock = null;
    }
}
