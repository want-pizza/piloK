using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSFX : MonoBehaviour
{
    [SerializeField] private AudioClip source;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            AudioManager.Instance.PlaySFX(source);
        }
    }
}
