using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //�䵽�����л�Ϊ��ֹ״̬
        if (player.isGround)
        {
            stateMachine.ChangeState(player.idleState);
        }

        //���е��ƶ��ٶ�
        if (xInput != 0)
            player.SetVelocity(player.speed * xInput, rb.velocity.y);
    }
}
