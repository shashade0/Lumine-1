using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = 10;
        player.isJump = true;
        rb.velocity = new Vector2(rb.velocity.x, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(xInput != 0)
            rb.velocity = new Vector2(xInput * player.speed * 1.2f, rb.velocity.y);

        if (rb.velocity.y < 0)
            stateMachine.ChangeState(player.airState);

        if (player.isJump && player.canGrab && player.isWall && !player.isGround)
            stateMachine.ChangeState(player.grabState);
    }
}
