using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingTextSpawner : MonoBehaviour
{
    public static FloatingTextSpawner Instance;

    [SerializeField] private UnityEngine.GameObject prefab;

    private void Awake()
    {
        Instance = this;
    }

    public void Spawn(string text, Vector3 pos)
    {
        pos.y += 1f;
        pos.z -= 1f;
        var ps = Instantiate(prefab, pos, Quaternion.identity);
        ps.GetComponentInChildren<TextMesh>().text = text;
        Debug.Log(pos);
        //need object pull
        Destroy(ps.gameObject, 0.5f);
    }
}
