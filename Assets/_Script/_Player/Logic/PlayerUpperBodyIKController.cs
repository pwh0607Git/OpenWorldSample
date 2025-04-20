using UnityEngine;
using CustomInspector;

public class PlayerUpperBodyIKController : MonoBehaviour
{
    private Camera mainCam;
    private Animator animator;
    [SerializeField] Transform sightPoint;

    [SerializeField] float bodyWeight, headWeight;
    [SerializeField] float eyePoint;

    // 카메라의 방향에 따라서...
    void Start()
    {
        mainCam = Camera.main;
        TryGetComponent(out animator);
    }

    //layerIndex =>
    // OnAnimatorIK는 IK의 변화를 인지하고 처리한다. 즉, layerIndex를 조건문을 통하여 구분하고 그에 맞도록 처리한다.
    void OnAnimatorIK(int layerIndex)
    {
        if(animator == null || mainCam == null) return;


        if(layerIndex == 2){
            //  float weight, float bodyWeight, float headWeight, float eyesWeight, float clampWeight
            animator.SetLookAtWeight(1.0f, bodyWeight, headWeight);

            Vector3 lookDir = mainCam.transform.up;
            Debug.Log($"{sightPoint.localPosition.y}");
            lookDir.y = sightPoint.localPosition.y;              // Player의 눈높이로 설정하기.

            animator.SetLookAtPosition(lookDir);
        }
    }
}
