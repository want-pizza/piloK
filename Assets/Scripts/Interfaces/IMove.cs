using UnityEngine;

public interface IMove : ICanBePaused
{
    public float XVelocity { get; }
    public float YVelocity { get; }
    public void TakeEfficiency(Vector2 direction, float power);
}
