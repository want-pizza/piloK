using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : IState
{
    //PlayerMovement movement;
    //PlayerInventoryPresenter inventoryPresenter;
    //PlayerWeaponController controller;
    protected PlayerStateMachine stateMachine;
    protected Transition[] transitions;
    protected string animationName;

    public virtual void OnEnter()
    {
        //Debug.Log($"OnEnter; transitions = {transitions.Count}");
        foreach (var transition in transitions)
        {
            Debug.Log($"{transition.ToString()}");
            transition.OnEnable();
        }
    }
    public virtual void OnExit()
    {
        foreach (var transition in transitions)
        {
            transition.OnDisable();
        }
    }
}
