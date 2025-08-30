using System;
using UnityEngine;
using UnityEngine.Events;

public class IrisListener : MonoBehaviour
{
    [Header("Iris Settings")]
    [SerializeField] private Transform target;
    [SerializeField] private Material material;
    [SerializeField] private float time = 1f;
    [SerializeField] private float speed = 2f;
    [Header("Callbacks")]
    [SerializeField] private UnityEvent onDark;
    [SerializeField] private UnityEvent onComplete;

    public Transform Target { get { return target; } set { target = value; } }
    public void PlayIris()
    {
        IrisPlayer.Instance.PlayIris(target,
           onDark: () => onDark.Invoke(),
           onComplete: () => onComplete.Invoke()
        );
    }
}
