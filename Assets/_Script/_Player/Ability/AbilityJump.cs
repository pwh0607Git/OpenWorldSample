using UnityEngine;
public class AbilityJump : Ability<AbilityJumpData>
{
    public override AbilityFlag Flag => AbilityFlag.Jump;
    private bool jumping = false;
    float elapsedTime = 0f;
    public AbilityJump(AbilityJumpData data, PlayerController1 player) : base(data, player) { }
    public override void FixedUpdate()
    {
        Jump();
    }

    public override void Activate()
    {
        jumping = true;
        elapsedTime = 0;
    }

    public override void Deactivate()
    {
        jumping = false;
        elapsedTime = 0;
    }

    void Jump(){
        if(player.controller == null || !jumping) return;

        elapsedTime += Time.deltaTime;

        float t = elapsedTime / data.duration;
        float height = data.jumpCurve.Evaluate(t) * data.jumpForce;
        player.controller.Move(Vector3.up * height * Time.deltaTime);
        
        if(elapsedTime > data.duration || (elapsedTime > 0.1f && player.isGrounded)){
            jumping = false;
            elapsedTime = 0.0f;
        }
    }
}