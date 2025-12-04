using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected Vector2 impulseVelocity;
    protected Vector3 targetPosition;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float spread = 0f;

    public virtual void Launch(Vector3 target)
    {
        targetPosition = target;
        targetPosition.x += Random.Range(-spread, spread);
        targetPosition.y += Random.Range(-spread, spread);
    }

    protected abstract void Move();

    protected abstract void OnImpact(bool value);
}
