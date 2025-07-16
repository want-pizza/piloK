using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    private Field<float> xVelocity;
    public PlayerRunState(PlayerStateMachine _stateMachine, List<Transition> _transitions, Field<float> _XVelocity, string _animationName) 
    {
        stateMachine = _stateMachine;
        transitions = _transitions;
        xVelocity = _XVelocity;
        animationName = _animationName;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        xVelocity.OnValueChanged += OnSpeedChanged;
        stateMachine.PlayerAnimation(animationName, 0.1f);
    }
    public override void OnExit()
    {
        base.OnExit();
        xVelocity.OnValueChanged -= OnSpeedChanged;
    }
    private void OnSpeedChanged(float _speed)
    {
        float speed = Mathf.Clamp01(_speed); // need tests
        stateMachine.ChangeAnimationSpeed(speed);
    }
}
