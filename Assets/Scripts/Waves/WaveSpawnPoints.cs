using System.Collections.Generic;
using UnityEngine;

public class WaveSpawnPoints : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    private void Awake()
    {
        points.Clear();
        foreach (Transform t in transform)
            points.Add(t);
    }
}
