using UnityEngine;

public abstract class BaseItemData : ScriptableObject, IPickupable, IItem
{
    [Header("Meta Info")]
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip pickupSound;

    public Sprite Icon => icon;
    public AudioClip PickupSound => pickupSound;
    public string DisplayName => displayName;

    public abstract void OnPickup(PlayerStats playerStats);
    public abstract void Apply(PlayerStats playerStats); // נואכ³חאצ³ÿ IItem
}
