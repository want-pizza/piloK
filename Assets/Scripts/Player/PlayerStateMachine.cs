using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                            inventoryPresenter.IsOpen);
        Transition wallSliceTrasition = new WallSlideTransition(
                            this,
                            movement.FieldIsGrounded,
                            movement.FieldIsTouchingLeftWall,
                            movement.FieldIsTouchingRightWall,
                            movement.FieldVelocityY);

        states.Add(typeof(PlayerIdleState), new PlayerIdleState(this, "Idle", runTransition));
        states.Add(typeof(PlayerRunState), new PlayerRunState(this, movement.FieldVelocityX, "Run", idleTransition, wallSliceTrasition));
        states.Add(typeof(PlayerWallSlideState), new PlayerWallSlideState(this, "WallSlide", idleTransition));
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
        Debug.Log($"animator.speed - {animator.speed}; speed - {speed}");
    }
}
