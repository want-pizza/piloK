using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour, IHitable
{
    [SerializeField] AudioClip clip;
    public void PlaySFX()
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}
