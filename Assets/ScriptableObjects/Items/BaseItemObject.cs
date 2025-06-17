using UnityEngine;

public abstract class BaseItemObject : ScriptableObject
{
    [Header("Meta Info")]
    [SerializeField] private string displayName;
    [SerializeField] private Sprite icon;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private bool isStackable;
    [TextArea(15,20)]
    [SerializeField] private string description;


    public Sprite Icon => icon;
    public AudioClip PickupSound => pickupSound;
    public string DisplayName => displayName;
    public bool IsStackable => isStackable;
    public string Description => description;
}
