using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour, IPickupable
{

    [SerializeField] private BaseItemObject itemData;
    [SerializeField] private Vector2 triggerSize = new Vector2(1f, 1f);
    [SerializeField] private bool useBoxCollider = true;
    [SerializeField] private int amount = 1;

    public Sprite GetIcon => itemData.Icon;

    public AudioClip GetPickupSound => itemData.PickupSound;

    public string GetDisplayName => itemData.DisplayName;

    public string GetDescription => itemData.Description;

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
        if (other.TryGetComponent(out PlayerInventory inventory))
        {
            OnPickup(inventory);

            if (itemData.PickupSound != null)
                AudioSource.PlayClipAtPoint(itemData.PickupSound, transform.position);

            Destroy(gameObject);
        }
    }

    public void OnPickup(PlayerInventory inventory)
    {
        inventory.TryPickupItem(itemData, amount);
    }
}
