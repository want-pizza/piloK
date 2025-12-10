using UnityEngine;

public class RigidbodyPauser : MonoBehaviour, ICanBePaused
{
    private Rigidbody rb;
    private Vector3 savedVelocity;
    private Vector3 savedAngularVelocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnPausedChanged(bool paused)
    {
        if (paused)
        {
            savedVelocity = rb.velocity;
            savedAngularVelocity = rb.angularVelocity;

            rb.isKinematic = true;
        }
        else
        {
            rb.isKinematic = false;

            rb.velocity = savedVelocity;
            rb.angularVelocity = savedAngularVelocity;
        }
    }
}
