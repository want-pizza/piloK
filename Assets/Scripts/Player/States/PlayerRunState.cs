using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    private Field<float> xVelocity;
    public PlayerRunState(PlayerStateMachine _stateMachine, Field<float> _XVelocity, string _animationName, params Transition[] _transitions) 
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
        stateMachine.PlayAnimation(animationName);
        Debug.Log("OnEnter - PlayerRunState");
    }
    public override void OnExit()
    {
        base.OnExit();
        xVelocity.OnValueChanged -= OnSpeedChanged;
        
        stateMachine.ChangeAnimationSpeed(1f);
        Debug.Log("OnExit - PlayerRunState");
    }
    private void OnSpeedChanged(float _speed)
    {
        float speed = Mathf.InverseLerp(-0.1f, 5f, Mathf.Abs(_speed)); // need tests
        stateMachine.ChangeAnimationSpeed(speed);
    }
}
