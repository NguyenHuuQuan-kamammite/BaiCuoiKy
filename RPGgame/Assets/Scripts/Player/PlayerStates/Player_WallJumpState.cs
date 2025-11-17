using UnityEngine;

public class Player_WallJumpState : PlayerState
{
    private Entity_SFX sfx;
    public Player_WallJumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        sfx = player.GetComponent<Entity_SFX>();
    }

    public override void Enter()
    {
        base.Enter();
        sfx?.PlayJump();
        player.SetVelocity(player.wallJumpForce.x * -player.facingDir, player.wallJumpForce.y);
    }

    public override void Update()
    {
        base.Update();

        if (rb.linearVelocity.y < 0)
            stateMachine.ChangeState(player.fallState);

        if (player.wallDetected)
            stateMachine.ChangeState(player.wallSlideState);
    }
}
