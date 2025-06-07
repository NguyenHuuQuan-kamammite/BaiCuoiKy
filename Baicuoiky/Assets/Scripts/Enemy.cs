using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    [Header("Battle Details")]
    public float battleMoveSpeed = 3f;
    public float attackDistance = 2f;

    [Header("Movement Details")]
    public float moveSpeed = 1.4f;
    public float idleTime = 1f;

    [Range(0, 2)]
    public float moveAnimSpeedMultiplier = 1f;


    [Header("Player Detection")]
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10f;
    public RaycastHit2D PlayerDetected()

    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Debug.DrawRay(playerCheck.position, Vector2.right * facingDir * playerCheckDistance, Color.red);
            return default;
        }
        
       return hit;
    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (playerCheckDistance * facingDir), playerCheck.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (attackDistance * facingDir), playerCheck.position.y));
    }
}
