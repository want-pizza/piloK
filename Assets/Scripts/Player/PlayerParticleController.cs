using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleController : MonoBehaviour
{
    [System.Serializable]
    public class ParticleEntry
    {
        public string key;
        public ParticleSystem particle;
    }

    [SerializeField] private List<ParticleEntry> particles = new List<ParticleEntry>();
    private Dictionary<string, ParticleSystem> particleDict;

    private void Awake()
    {
        particleDict = new Dictionary<string, ParticleSystem>();
        foreach (var entry in particles)
        {
            if (!particleDict.ContainsKey(entry.key) && entry.particle != null)
            {
                particleDict.Add(entry.key, entry.particle);
            }
        }
    }

    public void Play(string key)
    {
        if (particleDict.TryGetValue(key, out var ps))
        {
            ps.Play();
        }
        else
        {
            Debug.LogWarning($"Particle with key '{key}' not found!");
        }
    }

    public void Stop(string key)
    {
        if (particleDict.TryGetValue(key, out var ps))
        {
            ps.Stop();
        }
    }

    public void Pause(string key)
    {
        if (particleDict.TryGetValue(key, out var ps))
        {
            ps.Pause();
        }
    }
}
