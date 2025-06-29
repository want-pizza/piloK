using UnityEngine;

public abstract class BaseItemObject : ScriptableObject
{
    [Header("Meta Info")]
    [SerializeField] private int id;
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private bool isStackable;
    [TextArea(15,20)]
    [SerializeField] private string description;

    public virtual void Use(PlayerStats stats) { }
    public virtual bool CanEquip => false;
    public virtual bool CanUnequip => true;
    public virtual bool IsEquipped => false;
    public virtual void Equip(PlayerStats stats) { }
    public virtual void Unequip(PlayerStats stats) { }


    public int Id => id;
    public Sprite Icon => icon;
    public AudioClip PickupSound => pickupSound;
    public string DisplayName => displayName;
    public bool IsStackable => isStackable;
    public string Description => description;
}
