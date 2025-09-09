
using UnityEngine;

public class Player_ThrowSwordState : PlayerState
{
    private Camera mainCamera;
    public Player_ThrowSwordState(Player player, StateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {
    }
    public override void Enter()
    {
        base.Enter();
        skillManager.swordThrow.EnableDots(true);
        if (mainCamera != Camera.main)
            mainCamera = Camera.main;
        
    }
    public override void Update()
    {

        base.Update();
        Vector2 dirToMouse = DirectionToMouse();
        player.HandleFlip(dirToMouse.x);
        player.SetVelocity(0, rb.linearVelocity.y);
        player.skillManager.swordThrow.PredictTrajectory(dirToMouse);
        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);
            skillManager.swordThrow.EnableDots(false);
            player.skillManager.swordThrow.ConfirmTrajectory(dirToMouse);
        }
        if (input.Player.Attack.WasReleasedThisFrame() || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
    public override void Exit()
    {
        base.Exit();
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }
    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);


        Vector2 direction = worldMousePosition - playerPosition;
        return direction.normalized;

    }
    
}
