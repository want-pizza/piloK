using UnityEngine;

public interface IPickupable
{
    Sprite Icon { get; }
    AudioClip PickupSound { get; }
    string DisplayName { get; }
    void OnPickup(PlayerStats playerStats);
}
