using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PickupItem : MonoBehaviour, IPickupable
{

    [SerializeField] private BaseItemObject itemData;
    [SerializeField] private Vector2 triggerSize = new Vector2(1f, 1f);
    [SerializeField] private int amount = 1;

    public Sprite GetIcon => itemData.Icon;

    public AudioClip GetPickupSound => itemData.PickupSound;

    public string GetDisplayName => itemData.DisplayName;

    public string GetDescription => itemData.ShopDescription;

    private void Reset()
    {
        SetupCollider();
    }

    private void Awake()
    {
        SetupCollider();
        gameObject.GetComponent<SpriteRenderer>().sprite = GetIcon;
    }

    private void SetupCollider()
    {
        Collider2D collider = GetComponent<Collider2D>();

        if (collider == null)
        {
            var box = gameObject.AddComponent<BoxCollider2D>();
            box.size = triggerSize;
            box.isTrigger = true;
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
        if (other.TryGetComponent(out PlayerInventoryPresenter inventory))
        {
            bool added = inventory.TryPickupItem(itemData, amount);

            if (added)
            {
                if (itemData.PickupSound != null)
                    AudioManager.Instance.PlaySFX(itemData.PickupSound);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Inventory full! Can't pick up the item.");
            }
        }
    }

    public void OnPickup(PlayerInventoryPresenter inventory)
    {
        inventory.TryPickupItem(itemData, amount);
    }
}
