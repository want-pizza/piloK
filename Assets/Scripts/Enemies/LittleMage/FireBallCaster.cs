using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireBallCaster : MonoBehaviour
{
    [SerializeField] private string fireBallKey;
    [SerializeField] private Transform attackPoint;
    private Transform targetTransform;
    private ProjectileBase projectile;

    private void Start()
    {
        targetTransform = FindAnyObjectByType<PlayerMovement>().transform;
    }
    public void CastFireBall(Transform _targetTransform = null)
    {
        if (_targetTransform != null)
            targetTransform = _targetTransform;

        projectile = ProjectileManager.Instance.SpawnProjectile(fireBallKey, attackPoint.position, targetTransform.position);
    }
}
