using UnityEngine;

public class Player_MoveState : Player_GroundedState
{
    private Entity_SFX sfx;
    private float stepTimer;
    private float stepInterval = 0.4f; // Adjust this based on walk speed

    public Player_MoveState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
        sfx = player.GetComponent<Entity_SFX>();
    }

    public override void Enter()
    {
        base.Enter();
        stepTimer = 0f; // Reset timer when entering move state
    }

    public override void Update()
    {
        base.Update();

        // Play footstep sound at intervals
        stepTimer -= Time.deltaTime;
        if (stepTimer <= 0f)
        {
            sfx?.PlayWalkStone();
            stepTimer = stepInterval;
        }

        if (player.moveInput.x == 0 || player.wallDetected)
            stateMachine.ChangeState(player.idleState);

        player.SetVelocity(player.moveInput.x * player.moveSpeed, rb.linearVelocity.y);
    }

}
