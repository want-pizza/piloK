using UnityEngine;
using System.Collections.Generic;

public class FXManager : MonoBehaviour
{
    public static FXManager Instance;

    [System.Serializable]
    public class FXEntry
    {
        public string key;
        public ParticleSystem prefab;
    }

    [SerializeField] private List<FXEntry> fxList;
    private Dictionary<string, ParticleSystem> fxDict;

    private void Awake()
    {
        Instance = this;

        fxDict = new Dictionary<string, ParticleSystem>();
        foreach (var entry in fxList)
        {
            if (!fxDict.ContainsKey(entry.key) && entry.prefab != null)
                fxDict.Add(entry.key, entry.prefab);
        }
    }

    public void Play(string key, Vector3 pos, Quaternion rot)
    {
        if (!fxDict.TryGetValue(key, out var prefab))
        {
            Debug.LogWarning($"FX '{key}' not found!");
            return;
        }

        var ps = Instantiate(prefab, pos, rot);
        ps.Play();

        //need object pull
        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
    }
}
