using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : IState
{
    protected PlayerStateMachine stateMachine;
    protected TransitionBase[] transitions;
    protected string animationName;
    //protected string boolValueName;
    public virtual bool CanMove() => true;
    public virtual bool CanDash() => true;
    public virtual bool CanOpenInventory() => false;

    public virtual void OnEnter()
    {
        //Debug.Log($"OnEnter; transitions = {transitions.Count}");
        foreach (var transition in transitions)
        {
            //Debug.Log($"{transition.ToString()}");
            transition.OnEnable();
        }
    }
    public virtual void OnEnter(string boolValueName)
    {
        //Debug.Log($"OnEnter; transitions = {transitions.Count}");
        foreach (var transition in transitions)
        {
            //Debug.Log($"{transition.ToString()}");
            transition.OnEnable();
        }
        SelectTransitionAnimation(boolValueName);
    }
    public virtual void OnExit()
    {
        foreach (var transition in transitions)
        {
            transition.OnDisable();
        }
    }
    protected void SelectAnimation()
    {
        if (animationName != null)
            stateMachine.PlayAnimation(animationName);
        else Debug.LogError("Animation not selected");
    }
    protected void SelectTransitionAnimation(string transitionAnimationName)
    {
        if (transitionAnimationName != null)
            stateMachine.PlayAnimation(transitionAnimationName);
        else Debug.LogError("transitionAnimation not selected");
    }
    public void TransitionAnimationEnded()
    {
        SelectAnimation();
    }
}
