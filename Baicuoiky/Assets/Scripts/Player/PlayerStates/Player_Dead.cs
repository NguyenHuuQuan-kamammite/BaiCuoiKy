using UnityEngine;

public class Player_Dead : PlayerState
{
    public Player_Dead(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        input.Disable();
        rb.simulated = false;
    }
}
