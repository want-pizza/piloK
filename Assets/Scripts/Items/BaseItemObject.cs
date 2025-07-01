using UnityEngine;

public abstract class BaseItemObject : ScriptableObject
{
    [Header("Meta Info")]
    [SerializeField] private int id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private bool canEquip;
    [SerializeField] private bool canUnequip;
    [SerializeField] private bool isStackable;

    public virtual void Use(PlayerStats stats) { }
    
    public int Id => id;
    public Sprite Icon => icon;
    public AudioClip PickupSound => pickupSound;
    public string DisplayName => displayName;
    public bool IsStackable => isStackable;
    public string Description => description;

    public bool CanUnequip => canUnequip;
    public bool CanEquip => canEquip;

    [TextArea(15, 20)]
    [SerializeField] private string description;
}
