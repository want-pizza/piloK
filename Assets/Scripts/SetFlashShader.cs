using UnityEngine;

public class SetFlashShader : MonoBehaviour
{
    [SerializeField] private Material flashMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Material defaultMaterial;

    void Awake()
    {
        defaultMaterial = spriteRenderer.material;
    }
    public void SetFlashMaterial()
    {
        if (spriteRenderer == null || flashMaterial == null) return;

        Material matInstance = new Material(flashMaterial);
        matInstance.SetColor("_Color", Color.white);

        spriteRenderer.material = matInstance;
    }
    public void ReSetFlashMaterial()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.material = defaultMaterial;
    }
}
