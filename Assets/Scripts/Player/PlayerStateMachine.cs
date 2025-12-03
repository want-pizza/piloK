using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerStateMachine : MonoBehaviour, IStateMachine
{
    private Field<PlayerState> currentState = new Field<PlayerState>();
    private Dictionary<Type, IState> states= new Dictionary<Type, IState>();
    [SerializeField] public Animator animator;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private DashTrailController dashTrailController;
    [SerializeField] private PlayerInventoryPresenter inventoryPresenter;
    [SerializeField] private PlayerLifeCircle lifeCircle;
    [SerializeField] private PlayerCombat playerCombat;
    public Field<PlayerState> CurrentState => currentState;

    public void ChangeState<T>() where T : IState
    {
        currentState.Value.OnExit();

        if (states.TryGetValue(typeof(T), out IState newState))
        {
            currentState.Value = (PlayerState)newState;
            currentState.Value.OnEnter();
            //Debug.Log($"current state - {typeof(T)}");
        }
        else
        {
            Debug.LogError($"State of type {typeof(T)} not found!");
        }
    }
    public void ChangeState<T>(string boolVariableName) where T : IState
    {
        currentState.Value.OnExit();

        if (states.TryGetValue(typeof(T), out IState newState))
        {
            currentState.Value = (PlayerState)newState;
            currentState.Value.OnEnter(boolVariableName);
            //Debug.Log($"current state - {typeof(T)}");
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
        currentState.Value = (PlayerState)states[typeof(PlayerIdleState)];
        currentState.Value.OnEnter();
    }

    private void CreateStatesAndTransitions()
    {
        TransitionBase runTransition = new RunTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing);
        TransitionBase idleTransition = new IdleTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            lifeCircle.FieldIsDead);
        //Transition wallSliceTrasition = new WallSlideTransition(
        //                    this,
        //                    movement.FieldIsGrounded,
        //                    movement.FieldIsTouchingLeftWall,
        //                    movement.FieldIsTouchingRightWall,
        //                    movement.FieldVelocityY);
        TransitionBase jumpTransition = new JumpTransition(
                            this,
                            movement.FieldIsJumping,
                            movement.FieldIsGrounded,
                            "StartJumping");
        TransitionBase flyingUpwardTransition = new FlyingUpwardTransition(
                            this,
                            movement.FieldIsGrounded,
                            movement.FieldVelocityY,
                            movement.FieldIsDashing);
        TransitionBase fallTransition = new FallTransition(
                            this,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            movement.FieldVelocityY);
        TransitionBase fallIdleTransition = new FallIdleTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            lifeCircle.FieldIsDead,
                            "Landing");
        TransitionBase fallRunTransition = new FallRunTransition(
                            this,
                            movement.FieldVelocityX,
                            movement.FieldIsGrounded,
                            movement.FieldIsDashing,
                            "Landing");
        TransitionBase dashTransition = new DashTransition(
                            this,
                            movement.FieldIsDashing);
        TransitionBase deathTransition = new DeathTrantision(
                            this,
                            lifeCircle.FieldIsDead);
        TransitionBase attackTransition = new AttackTransition(
                            this,
                            playerCombat.IsAttacking);
        TransitionBase attackExitTransition = new AttackExitTransition(
                            this,
                            playerCombat.IsAttacking);

        states.Add(typeof(PlayerIdleState), new PlayerIdleState(this, "Idle", deathTransition, runTransition, jumpTransition, fallTransition, dashTransition, flyingUpwardTransition, attackTransition));
        states.Add(typeof(PlayerRunState), new PlayerRunState(this, movement.FieldVelocityX, "Run", deathTransition, jumpTransition, fallTransition, idleTransition, dashTransition, attackTransition));
        //states.Add(typeof(PlayerWallSlideState), new PlayerWallSlideState(this, "WallSlide", idleTransition));
        states.Add(typeof(PlayerFlyingUpwardState), new PlayerFlyingUpwardState(this, "FlyingUpward", deathTransition, dashTransition, fallTransition, fallIdleTransition, fallRunTransition, attackTransition));
        states.Add(typeof(PlayerFallState), new PlayerFallState(this, "Falling", deathTransition, fallIdleTransition, fallRunTransition, dashTransition, attackTransition));
        states.Add(typeof(PlayerDashState), new PlayerDashState(this, "Dash", dashTrailController.StopParticleSystem, deathTransition, fallTransition, idleTransition, runTransition, flyingUpwardTransition, attackTransition));
        states.Add(typeof(PlayerAttackState), new PlayerAttackState(this, playerCombat.CurrentAttackAnim, deathTransition, attackExitTransition));
        states.Add(typeof(PlayerDeathState), new PlayerDeathState(this, "Death", idleTransition));

        states.Add(typeof(PlayerAnyState), new PlayerAnyState(this, deathTransition, idleTransition, attackTransition, runTransition, jumpTransition, fallTransition, dashTransition, flyingUpwardTransition));
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
        currentState.Value.TransitionAnimationEnded();
    }
    public void StartRespawnEffect()
    {

    }
}
