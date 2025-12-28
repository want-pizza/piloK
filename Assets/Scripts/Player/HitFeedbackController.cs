using UnityEngine;
using Cinemachine;
using System.Collections;

public class HitFeedbackController : MonoBehaviour
{
    public static HitFeedbackController Instance;

    [Header("Hit Stop")]
    [SerializeField] private float hitStopDuration = 0.05f;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseSource impulseSource;
    [SerializeField] private float normalShake = 0.5f;
    [SerializeField] private float critShake = 1.2f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayHitFeedback(DamageResult result)
    {
        StartCoroutine(HitStopCoroutine());
        PlayCameraShake(result.IsFatal);
    }

    private IEnumerator HitStopCoroutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = 1f;
    }

    private void PlayCameraShake(bool isCrit)
    {
        float power = isCrit ? critShake : normalShake;
        impulseSource.GenerateImpulse(power);
    }
}
