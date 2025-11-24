using UnityEngine;

public class FXBehaviour : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private float soundCooldown = 0.1f;

    private float cooldownTimer = 0f;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    void OnParticleCollision(GameObject other)
    {
        if (cooldownTimer > 0f)
            return;

        if (other.CompareTag("Ground"))
        {
            AudioManager.Instance.PlaySFX(hitSound, other.transform.position, 1f);
            cooldownTimer = soundCooldown;
        }
    }
}
