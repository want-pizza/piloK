using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine
{
    void ChangeState<T>() where T : IState;
}
