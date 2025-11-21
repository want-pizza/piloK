using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSFXPlayer : MonoBehaviour
{
    [Header("SFX Clips")]
    [SerializeField] private AudioClip[] audioClips;

    [Header("AttackEventChannelSO")]
    [SerializeField] private AttackEventChannel eventChannel;

    private void OnEnable()
    {
        eventChannel.OnSwingStart += PlayAudioClip;
    }
    private void OnDisable()
    {
        eventChannel.OnSwingStart -= PlayAudioClip;
    }
    private void PlayAudioClip(string name)
    {
        foreach (AudioClip clip in audioClips)
        {
            if(clip.name == name)
            {
                AudioManager.Instance.PlaySFX(clip);
                Debug.Log("sfx playing");
                break;
            }
        }
        Debug.LogWarning("SFX sound didn't found");
    }
}
