using UnityEngine;

/*
    Forest-Golem
    [기준 : Distance : 10f]
    1. 원거리 투척
    - 포물선으로 돌 투척
    - 
    2. 근거리 펀치
*/

public class BossController : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject throwAblePrefab;

    void Start(){
        target = GameObject.FindWithTag("Player");
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            ShootObject();
        }
    }

    void ShootObject(){
        GameObject throwAbleObject = Instantiate(throwAblePrefab);
        //throwAbleObject.OnEnable 시 move 작동.
    }
}

public class BossState{

}