using UnityEngine;

public class Test_Player : MonoBehaviour
{
    CharacterController controller;
    float gravity = 20.0f;
    float moveSpeed = 10.0f;
    Vector3 nextDestination;
    Vector3 originalPosition;
    void Start(){
        TryGetComponent(out controller);
        originalPosition = transform.position;
        SetNextDestination();
    }
    float waitTimer= 0.0f;
    float waitingTime = 3.0f;
    bool isWaiting = false;

    void Update(){     
        if(isWaiting){
            waitTimer -= Time.deltaTime;
            if(waitTimer <= 0f){
                isWaiting = false;
                SetNextDestination();
            }
        }
        Vector3 checkPosition = transform.position;
        checkPosition.y = 0;
        if(Vector3.Distance(checkPosition, nextDestination) <= 1f){
            if(!isWaiting){
                isWaiting = true;
                waitTimer = waitingTime;
            }
        }else{
            Move(nextDestination);
        }
    }

    void Move(Vector3 destination){

        Vector3 moveDirection = (destination - transform.position).normalized;
        if (controller.isGrounded)
        {
            if (moveDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
                moveDirection = transform.TransformDirection(moveDirection);
            }
        }
        
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        // transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * 10f);
    }

    private void SetNextDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10f;
        randomDirection += originalPosition;            //최초 위치를 기준으로 원을 그려서 이동 범위를 설정한다.
        nextDestination = randomDirection;
        nextDestination.y = 0;
        Debug.Log($"도착! 다음 목적지 설정...{nextDestination}");
    }
}
