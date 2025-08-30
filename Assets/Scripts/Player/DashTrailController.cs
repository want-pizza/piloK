using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class DashTrailController : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private SpriteRenderer playerSpriteRenderer;
    [SerializeField] private CharacterFacing characterFacing;

    private Vector2 uvOffset;
    private Vector2 uvScale;

    private ParticleSystemRenderer psRenderer;
    private Material particleMaterial;

    void Start()
    {
        psRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
        particleMaterial = psRenderer.material;
    }
    public void PlayParticleSystem()
    {
        Debug.Log("PlayPartickleSystem");
        UpdateUVParamsFromSprite(playerSpriteRenderer.sprite);

        particleMaterial.SetVector("_UVOffset", uvOffset);
        particleMaterial.SetVector("_UVScale", uvScale);
        //particleMaterial.SetFloat("_Transparency", 1f);

        psRenderer.flip = new Vector3(characterFacing.IsFacingRight ? 0f : 1f, 0f, 0f);

        particleSystem.Play();
    }
    public void StopParticleSystem()
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
