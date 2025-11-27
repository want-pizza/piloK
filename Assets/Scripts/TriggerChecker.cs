using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerChecker : MonoBehaviour
{
    public event Action<bool> OnTriggeredStateChanged;
    [SerializeField] LayerMask layerMask;
    [Header("Other")]
    [SerializeField] bool isReturnCollider = false;
    public event Action<Collider2D> OnTriggerEnter;

    private int triggerCount = 0;
    private bool _isTriggered = false;

    public bool IsTriggered
    {
        get => _isTriggered;
        private set
        {
            if (_isTriggered != value)
            {
                _isTriggered = value;
                //Debug.Log("OnTriggeredStateChanged?.Invoke");
                OnTriggeredStateChanged?.Invoke(_isTriggered);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"OnTriggerEnter2D - {(1 << collision.gameObject.layer)} = {layerMask.value}");
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (isReturnCollider)
            {
                OnTriggerEnter?.Invoke(collision);
            }
            triggerCount++;
            SetTrigger();
            return;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            triggerCount--;
            SetTrigger();
            return;
        }
    }
    private void SetTrigger()
    {
        IsTriggered = triggerCount > 0;
    }
}