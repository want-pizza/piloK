using UnityEngine;

public class Spear : MonoBehaviour
{
    private Transform owner;
    private Vector2 attackDir;
    private float damage;
    [SerializeField] private TriggerChecker groundChecker;

    public void Init(Transform owner, Vector2 dir, float damage)
    {
        this.owner = owner;
        this.attackDir = dir.normalized;
        this.damage = damage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IDamageable target))
        {
            target.TakeDamage((int)damage);

            if (attackDir == Vector2.down &&
                owner.TryGetComponent(out Rigidbody2D rb) &&
                !IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, 12f);
            }
        }
    }

    private bool IsGrounded()
    {
        return groundChecker.IsTriggered;
    }

    public void Attack(Vector2 direction, Transform owner)
    {
        throw new System.NotImplementedException();
    }

    public void PerformAttack(Animator animator)
    {
        throw new System.NotImplementedException();
    }

    public void Equip(GameObject player)
    {
        throw new System.NotImplementedException();
    }

    public void Unequip(GameObject player)
    {
        throw new System.NotImplementedException();
    }
}
