using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemVelocityInfluence : MonoBehaviour
{
    [SerializeField] private ParticleSystem flameParticles;
    [SerializeField] private float velocityInfluence = 0.2f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (flameParticles != null && rb != null)
        {
            var vel = rb.velocity;
            var velModule = flameParticles.velocityOverLifetime;
            velModule.enabled = true;

            velModule.x = vel.x * -velocityInfluence;
            velModule.y = vel.y * -velocityInfluence;
        }
    }

}
