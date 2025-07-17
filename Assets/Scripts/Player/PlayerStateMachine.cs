using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour, IStateMachine
{
    private PlayerState currentState;
    private Dictionary<Type, IState> states= new Dictionary<Type, IState>();
    private Dictionary<Type, Transition> transitions = new Dictionary<Type, Transition>();
    [SerializeField] public Animator animator;
    [SerializeField] private PlayerMovement movement;

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

        transitions.Add(typeof(RunTransition), runTransition);
        List<Transition> tempTransitionList = new List<Transition>();
        tempTransitionList.Add(runTransition);
        states.Add(typeof(PlayerIdleState), new PlayerIdleState(this, "Idle", runTransition));
        states.Add(typeof(PlayerRunState), new PlayerRunState(this, movement.FieldVelocityX, "Run"));
    }

    public void PlayAnimation(string name)
    {
        animator.Play(name);
    }
    public void PlayAnimation(string name, float speed)
    {
        animator.Play(name, 1, speed);
    }

    internal void ChangeAnimationSpeed(float speed)
    {
        animator.speed = speed;
    }
}
