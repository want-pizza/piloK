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

    public void Spawn(string text, Vector3 pos, bool isCrit, bool isHeal)
    {
        float timeToDestroy = 0.5f;
        pos.y += 1f;
        pos.z -= 1f;
        var ps = Instantiate(prefab, pos, Quaternion.identity);
        ps.GetComponentInChildren<TextMesh>().text = text;

        if (isCrit)
        {
            Animator animator = ps.GetComponentInChildren<Animator>();
            animator.SetBool("isCrit", isCrit);
            animator.SetFloat("animSpeed", 0.5f);
            timeToDestroy *= 2;
        }
        if(isHeal)
            ps.GetComponentInChildren<Animator>().SetBool("isHeal", isHeal);
        Debug.Log(pos);
        //need object pull
        Destroy(ps.gameObject, timeToDestroy);
    }
}
