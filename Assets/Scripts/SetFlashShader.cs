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

        UpdateUVParamsFromSprite(spriteRenderer.sprite, matInstance);
        spriteRenderer.material = matInstance;
    }
    public void ReSetFlashMaterial()
    {
        if (spriteRenderer == null) return;
        spriteRenderer.material = defaultMaterial;
    }

    void UpdateUVParamsFromSprite(Sprite sprite, Material mat)
    {
        if (sprite == null || mat == null) return;

        Rect texRect = sprite.textureRect;
        Texture tex = sprite.texture;

        Vector2 uvOffset = new Vector2(texRect.x / tex.width, texRect.y / tex.height);
        Vector2 uvScale = new Vector2(texRect.width / tex.width, texRect.height / tex.height);

        mat.SetVector("_UVOffset", uvOffset);
        mat.SetVector("_UVScale", uvScale);
    }

}
