using UnityEngine;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    [Header("Movement Details")]
    public float moveSpeed = 1.4f;
    public float idleTime = 1f;

[Range(0,2)]
    public float moveAnimSpeedMultiplier = 1f;
}
