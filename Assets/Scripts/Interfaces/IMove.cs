using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMove : ICanBePaused
{
    public float XVelocity { get; }
    public float YVelocity { get; }
}
