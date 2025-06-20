using UnityEngine;

public interface IPickupable
{
    Sprite GetIcon { get; }
    AudioClip GetPickupSound { get; }
    string GetDisplayName { get; }
    string GetDescription { get; }
    void OnPickup(PlayerInventoryPresenter inventory);
}
