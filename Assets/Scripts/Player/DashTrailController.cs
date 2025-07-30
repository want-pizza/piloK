using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class DashTrailController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;

    private Vector2 uvOffset;
    private Vector2 uvScale;

    private Material particleMaterial;

    void Start()
    {
        var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        particleMaterial = renderer.material;
    }
    public void PlayPartickleSystem()
    {
        UpdateUVParamsFromSprite(playerSpriteRenderer.sprite);

        particleMaterial.SetVector("_UVOffset", uvOffset);
        particleMaterial.SetVector("_UVScale", uvScale);
        particleSystem.Play();
    }
    public void StopPartickleSystem()
    {
        Debug.Log("StopPartickleSystem");
        particleSystem.Stop();
    }
    void UpdateUVParamsFromSprite(Sprite sprite)
    {
        Rect texRect = sprite.textureRect;
        Texture tex = sprite.texture;

        uvOffset = new Vector2(texRect.x / tex.width, texRect.y / tex.height);
        uvScale = new Vector2(texRect.width / tex.width, texRect.height / tex.height);
    }
}
