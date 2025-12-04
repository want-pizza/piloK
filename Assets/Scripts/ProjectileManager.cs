using UnityEngine;
using System.Collections.Generic;
using Cinemachine.Utility;

public class ProjectileManager : MonoBehaviour
{
    public static ProjectileManager Instance { get; private set; }

    [System.Serializable]
    public struct ProjectilePrefab
    {
        public string key;
        public GameObject prefab;
    }

    [Header("Projectile prefabs dictionary")]
    public List<ProjectilePrefab> projectileList = new();

    private Dictionary<string, GameObject> projectiles = new();

    private void Awake()
    {
        Instance = this;

        foreach (var entry in projectileList)
        {
            if (entry.prefab == null)
            {
                Debug.LogWarning($"Projectile with key '{entry.key}' has empty prefab!");
                continue;
            }

            if (!projectiles.ContainsKey(entry.key))
                projectiles.Add(entry.key, entry.prefab);
            else
                Debug.LogWarning($"Duplicate projectile key '{entry.key}' ignored.");
        
        }
        DontDestroyOnLoad(gameObject);
    }

    public ProjectileBase SpawnProjectile(string key, Vector3 spawnPoint, Vector3 target)
    {
        if (!projectiles.TryGetValue(key, out GameObject prefab))
        {
            Debug.LogWarning($"Projectile '{key}' not found in dictionary!");
            return null;
        }

        Vector3 dir = (target - spawnPoint).normalized;

        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, -dir);

        GameObject projectile = Instantiate(prefab, spawnPoint, rotation);
        projectile.GetComponent<ProjectileBase>().Launch(target);

        return projectile.GetComponent<ProjectileBase>();
    }
}
