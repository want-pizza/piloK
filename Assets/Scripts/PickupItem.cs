using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour
{
    [SerializeField] private BaseItemData itemData;
    [SerializeField] private Vector2 triggerSize = new Vector2(1f, 1f);
    [SerializeField] private bool useBoxCollider = true;

    private void Reset()
    {
        SetupCollider();
    }

    private void Awake()
    {
        SetupCollider();
    }

    private void SetupCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider == null)
        {
            if (useBoxCollider)
            {
                var box = gameObject.AddComponent<BoxCollider2D>();
                box.size = triggerSize;
                box.isTrigger = true;
            }
            else
            {
                var circle = gameObject.AddComponent<CircleCollider2D>();
                circle.radius = Mathf.Max(triggerSize.x, triggerSize.y) / 2f;
                circle.isTrigger = true;
            }
        }
        else
        {
            collider.isTrigger = true;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
        rb.gravityScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerStats playerStats))
        {
            itemData.OnPickup(playerStats);

            if (itemData.PickupSound != null)
                AudioSource.PlayClipAtPoint(itemData.PickupSound, transform.position);

            Destroy(gameObject);
        }
    }
}
