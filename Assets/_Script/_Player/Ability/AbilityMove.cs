using UnityEngine;

public class AbilityMove : Ability<PlayerState>
{
    float horz, vert;
    private float _velocity;
    private Transform camTransform;
    private Vector3 direction;

    private RaycastHit hit;
    public float maxSlopeAngle = 45f;
    public float gravity = 30f;
    public AbilityMove(PlayerState data, PlayerController1 player) : base(data, player)
    {
        camTransform = Camera.main.transform; 
        _velocity = data.state.speed;               //임시로 움직임 스피드. 
    }

    public override void FixedUpdate()
    {
        InputKeyboard();
        Rotate();
        Move();
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Deactivate()
    {
        base.Deactivate();
    }

    private void InputKeyboard(){
        horz = Input.GetAxis("Horizontal");
        vert = Input.GetAxis("Vertical");
        
        Vector3 cameraForward = camTransform.forward.FlattenY();
        Vector3 cameraRight = camTransform.right.FlattenY();

        Vector3 movement = cameraRight * horz + cameraForward * vert;
        // direction = Vector3.ClampMagnitude(movement, 1);
        direction = movement.normalized;

        bool isOnSlope = CheckSlope();
        Vector3 adjustedMovement = isOnSlope ? AdjustDirectionToSlope(movement) : movement;

        if (player.isGrounded)
        {
            direction.y = -1;
            if (!isOnSlope) direction.y -= gravity * Time.deltaTime;
        }
        else direction.y -= gravity * Time.deltaTime;

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
        
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void Move(){
        player.controller.Move(direction * data.state.speed * Time.deltaTime);
    }
}
