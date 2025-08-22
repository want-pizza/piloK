using UnityEngine;

public class PlayParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystemToPlay;

    public void Play()
    {
        if (particleSystemToPlay == null) return;

        particleSystemToPlay.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particleSystemToPlay.Play();
    }
    public void Stop()
    {
        if (particleSystemToPlay == null) return;

        particleSystemToPlay.Stop();
    }
}
