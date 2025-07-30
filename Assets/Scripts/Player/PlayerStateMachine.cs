using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour, IStateMachine
{
    private PlayerState currentState;
    private Dictionary<Type, IState> states= new Dictionary<Type, IState>();
    [SerializeField] public Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;

    public void ChangeState<T>() where T : IState
    {
        currentState.OnExit();

        if (states.TryGetValue(typeof(T), out IState newState))
        {
            currentState = (PlayerState)newState;
            currentState.OnEnter();
            //Debug.Log($"current state - {typeof(T)}");
        }
        else
        {
            //Debug.LogError($"State of type {typeof(T)} not found!");
        }
    }
    public void ChangeState<T>(string boolVariableName) where T : IState
    {
        currentState.OnExit();

        if (states.TryGetValue(typeof(T), out IState newState))
        {
            currentState = (PlayerState)newState;
            currentState.OnEnter(boolVariableName);
            Debug.Log($"current state - {typeof(T)}");
        }
        else
        {
            Debug.LogError($"State of type {typeof(T)} not found!");
        }
    }
    void Start()
    {
        CreateStatesAndTransitions();
        Debug.Log("CreateStatesAndTransitions complete");
        EnterFirstState();
    }

    private void EnterFirstState()
    {
        currentState = (PlayerState)states[typeof(PlayerIdleState)];
        currentState.OnEnter();
    }

    private void CreateStatesAndTransitions()
    {
        Transition runTransition = new RunTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing);
        Transition idleTransition = new IdleTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            inventoryPresenter.IsOpen);
        //Transition wallSliceTrasition = new WallSlideTransition(
        //                    this,
        //                    movement.FieldIsGrounded,
        //                    movement.FieldIsTouchingLeftWall,
        //                    movement.FieldIsTouchingRightWall,
        //                    movement.FieldVelocityY);
        Transition jumpTransition = new JumpTransition(
                            this,
                            movement.FieldIsJumping,
                            movement.FieldIsGrounded,
                            "StartJumping");
        Transition flyingUpwardTransition = new FlyingUpwardTransition(
                            this,
                            movement.FieldIsGrounded,
                            movement.FieldVelocityY,
                            movement.FieldIsDashing);
        Transition fallTransition = new FallTransition(
                            this,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            movement.FieldVelocityY);
        Transition fallIdleTransition = new FallIdleTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            inventoryPresenter.IsOpen,
                            movement.FieldIsDashing,
                            "Landing");
        Transition fallRunTransition = new FallRunTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            "Landing");
        Transition dashTransition = new DashTransition(
                            this,
                            movement.FieldIsDashing);

        states.Add(typeof(PlayerIdleState), new PlayerIdleState(this, "Idle", runTransition, jumpTransition, fallTransition, dashTransition));
        states.Add(typeof(PlayerRunState), new PlayerRunState(this, movement.FieldVelocityX, "Run", jumpTransition, fallTransition, idleTransition, dashTransition));
        //states.Add(typeof(PlayerWallSlideState), new PlayerWallSlideState(this, "WallSlide", idleTransition));
        states.Add(typeof(PlayerFlyingUpwardState), new PlayerFlyingUpwardState(this, "FlyingUpward", fallTransition, fallIdleTransition, fallRunTransition, dashTransition));
        states.Add(typeof(PlayerFallState), new PlayerFallState(this, "Falling", fallIdleTransition, fallRunTransition, dashTransition));
        states.Add(typeof(PlayerDashState), new PlayerDashState(this, "Dash", fallTransition, idleTransition, runTransition, flyingUpwardTransition));
    }
    public bool IsVariableExist(string name)
    {
        //
        return false;
    }
    public void PlayAnimation(string name)
    {
        animator.CrossFade(name, 0f);
    }
    public void PlayAnimation(string name, float speed)
    {
        animator.Play(name, 0, speed);
    }
    public void ChangeAnimationSpeed(float speed)
    {
        animator.speed = speed;
        //Debug.Log($"animator.speed - {animator.speed}; speed - {speed}");
    }
    public void OnTransitionAnimationEnd()
    {
        currentState.TransitionAnimationEnded();
    }
}
