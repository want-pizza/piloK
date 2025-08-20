using System;
using System.Collections;
using UnityEngine;

public class IrisPlayer : MonoBehaviour
{
    public static IrisPlayer Instance { get; private set; }

    [Header("Default Iris Settings")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private float defaultIrisTime = 1f;
    [SerializeField] private float defaultIrisSpeed = 2f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayIris(Transform target, Action onDark = null, Action onComplete = null) =>
        StartCoroutine(IrisRoutine(target, defaultIrisTime, defaultIrisSpeed, defaultMaterial, onDark, onComplete));

    public void PlayIris(Transform target, float speed, Action onDark = null, Action onComplete = null) =>
        StartCoroutine(IrisRoutine(target, defaultIrisTime, speed, defaultMaterial, onDark, onComplete));

    public void PlayIris(Transform target, float time, float speed, Action onDark = null, Action onComplete = null) =>
        StartCoroutine(IrisRoutine(target, time, speed, defaultMaterial, onDark, onComplete));

    public void PlayIris(Transform target, float time, float speed, Material material, Action onDark = null, Action onComplete = null) =>
        StartCoroutine(IrisRoutine(target, time, speed, material ?? defaultMaterial, onDark, onComplete));

    private IEnumerator IrisRoutine(Transform target, float time, float speed, Material material, Action onDark, Action onComplete)
    {
        if (material == null)
        {
            Debug.LogWarning("IrisPlayer: no material assigned for iris effect");
            yield break;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(target.position);
        Vector2 irisCenter = new Vector2(screenPos.x, screenPos.y);
        material.SetVector("_IrisCenter", irisCenter);

        float darkness = 1f;
        while (darkness > 0f)
        {
            darkness -= Time.deltaTime * speed;
            material.SetFloat("_Darkness", Mathf.Clamp01(darkness));
            yield return null;
        }
        
        onDark?.Invoke();

        yield return new WaitForSeconds(time);

        screenPos = Camera.main.WorldToScreenPoint(target.position);
        irisCenter = new Vector2(screenPos.x, screenPos.y);
        material.SetVector("_IrisCenter", irisCenter);

        while (darkness < 1f)
        {
            darkness += Time.deltaTime * speed;
            material.SetFloat("_Darkness", Mathf.Clamp01(darkness));
            yield return null;
        }

        onComplete?.Invoke();
    }
}
