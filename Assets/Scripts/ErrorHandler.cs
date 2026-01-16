using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorHandler : MonoBehaviour
{
    public static ErrorHandler Instance { get; private set; }

    [SerializeField] private AudioClip errorSFX;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayErrorNoArgs()
    {
        AudioManager.Instance.PlaySFX(errorSFX);
    }
}
