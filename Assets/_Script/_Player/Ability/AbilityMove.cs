using UnityEngine;

public class AbilityMove : Ability<PlayerState>
{
    float horz, vert;
 
    private Transform camTransform;
    private Vector3 direction;

    private RaycastHit hit;
    public float maxSlopeAngle = 45f;
    public AbilityMove(PlayerState data, PlayerController1 player) : base(data, player)
    {
        camTransform = Camera.main.transform; 
    }

    public override void Update(){
        if(player.currentActivatedAbilities != AbilityFlag.Move) return;
        InputKeyboard();
        Rotate();
        Move();
        PlayAnimation();
    }

    public override void Activate(){ }

    public override void Deactivate() { }

    private void InputKeyboard(){
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");

        Vector3 cameraForward = camTransform.forward.FlattenY();
        Vector3 cameraRight = camTransform.right.FlattenY();

        Vector3 movement = cameraRight * horz + cameraForward * vert;
        direction = Vector3.ClampMagnitude(movement, 1);
        direction = movement.normalized;

        bool isOnSlope = CheckSlope();
        Vector3 adjustedMovement = isOnSlope ? AdjustDirectionToSlope(movement) : movement;
        
        if (player.isGrounded) direction.y = 0;

        direction.x = adjustedMovement.x;
        direction.z = adjustedMovement.z;
    }

    bool CheckSlope()
    {
        if (player.isGrounded)
        {
            Ray ray = new Ray(player.transform.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, player.controller.height / 2 * 1.1f))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > 0 && angle <= maxSlopeAngle;
            }
        }
        return false;
    }

    Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, hit.normal).normalized;
    }

    private void Rotate(){
        if (direction == Vector3.zero) return;
        
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
        if (flatDirection == Vector3.zero) return;  // 회전할 방향이 없으면 리턴

        Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void Move(){
        player.controller.Move(direction * 2f * Time.deltaTime);          // data.state.speed 
    }

    private void PlayAnimation(){
        float currentSpeed = Mathf.Clamp01(player.controller.velocity.magnitude);
        float speed = Mathf.Lerp(player.animator.GetFloat("MOVESPEED"), currentSpeed, Time.deltaTime * 5f);
        player.animator.SetFloat("MOVESPEED", speed);
    }
}
