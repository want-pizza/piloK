using UnityEngine;

public abstract class BaseItemObject : ScriptableObject
{
    [Header("Meta Info")]
    [SerializeField] private int id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private ItemRareness rareness;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private bool canEquip;
    [SerializeField] private bool canUnequip;
    [SerializeField] private bool isStackable;

    public int Id => id;
    public Sprite Icon => icon;
    public ItemRareness Rareness => rareness;
    public AudioClip PickupSound => pickupSound;
    public string DisplayName => displayName;
    public bool IsStackable => isStackable;
    public string Description => description;

    public bool CanUnequip => canUnequip;
    public bool CanEquip => canEquip;
    public virtual void OnEquip(PlayerContext context) { }
    public virtual void OnUnequip() { }

    [TextArea(15, 8)]
    [SerializeField] private string description;
}

[System.Serializable]
public enum ItemRareness
{
    Usual = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3
}
