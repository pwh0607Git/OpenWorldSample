using UnityEngine;
using CustomInspector;

public class PlayerUpperBodyIKController : MonoBehaviour
{
    private Camera mainCam;
    private Animator animator;
    [SerializeField] Transform sightPoint;

    [SerializeField] float bodyWeight, headWeight;
    [SerializeField] float eyePoint;

    float maxLookAngle = 120f;

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

        //Head
        if(layerIndex == 2){
        //    LookPos();
        } 
    }

    Vector3 ClampDirection(Vector3 fromDir, Vector3 toDir, float maxAngle)
    {
        float angle = Vector3.Angle(fromDir, toDir);
        if (angle <= maxAngle) return toDir;

        // 회전 제한
        Quaternion limitedRot = Quaternion.RotateTowards(
            Quaternion.LookRotation(fromDir),
            Quaternion.LookRotation(toDir),
            maxAngle
        );
        return limitedRot * Vector3.forward;
    }

    void LookPos(){
        //  float weight, float bodyWeight, float headWeight, float eyesWeight, float clampWeight

        Vector3 target = mainCam.transform.position + mainCam.transform.forward * 10f;
        target.y = sightPoint.localPosition.y;              

        //기본 트랜스 폼과 ik의 사이각을 제한.

        //사이각 계산하기 => float angle = Vector3.Angle(vec1, vec2);
        Vector3 lookDir = (target - transform.position).normalized;
        Vector3 forwardDir = transform.forward;

        Vector3 clampedDir = ClampDirection(forwardDir, lookDir, maxLookAngle * 0.5f);
        Vector3 lookAtPos = transform.position + clampedDir * 10f;

        animator.SetLookAtWeight(1.0f, bodyWeight, headWeight);
        animator.SetLookAtPosition(lookAtPos);
    }
}
