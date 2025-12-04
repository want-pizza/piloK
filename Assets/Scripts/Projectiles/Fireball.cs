using UnityEngine;
using UnityEngine.VFX;

public class Fireball : ProjectileBase
{
    [SerializeField] private string explosionEffectKey = null;
    [SerializeField] private TriggerChecker hitListener;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        hitListener.OnTriggeredStateChanged += OnImpact;
    }
    private void OnDisable()
    {
        hitListener.OnTriggeredStateChanged -= OnImpact;
    }
    public override void Launch(Vector3 target)
    {
        base.Launch(target);
        Vector3 dir = (targetPosition - transform.position).normalized;
        impulseVelocity = dir * speed;
        Move();
    }
    protected override void Move()
    {
        rb.AddForce(impulseVelocity, ForceMode2D.Impulse);
    }

    protected override void OnImpact(bool value)
    {
        if (explosionEffectKey != null)
            FXManager.Instance.Play(explosionEffectKey, transform.position, Quaternion.identity);

        Destroy(gameObject, 0.1f);
    }
}
