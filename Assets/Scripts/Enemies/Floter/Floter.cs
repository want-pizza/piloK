using System.Drawing;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody2D))]
public class Floter : MonoBehaviour, IMove
{
    [Header("Chasing Settings")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float turnRate = 3f;
    [SerializeField] private float separationRadius = 1.2f;
    [SerializeField] private float separationForce = 2f;
    [SerializeField] private LayerMask obstacleMask;


    [Header("Hit Settings")]
    [SerializeField] private float knockbackMultiplier = 2f;
    [SerializeField] private float efficiencyTime = 0.2f;
    [SerializeField] private float hitEfficiencyForce = 5f;
    [SerializeField] private AudioClip hitAudioClip;
    [SerializeField] private string vfxHitKey = "FloterHit";

    private Vector2 currentDir;

    private bool isEfficiency = false;
    private bool isPaused = false;

    private Rigidbody2D rb;
    [SerializeField] private DamageSource damageSource;
    private Transform player;

    public float XVelocity => rb.velocity.x;
    public float YVelocity => rb.velocity.y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb.velocity = Random.insideUnitCircle.normalized * speed;

    }

    private void OnEnable()
    {
        damageSource.OnDamageDealedNoArgs += TakeDamageHitEfficiency;
    }
    private void OnDisable()
    {
        damageSource.OnDamageDealedNoArgs -= TakeDamageHitEfficiency;
        isEfficiency = false;
    }

    private void TakeDamageHitEfficiency()
    {
        TakeEfficiency(-currentDir, hitEfficiencyForce);
    }

    private void FixedUpdate()
    {
        if (isPaused)
            return;

        if (isEfficiency)
            return;

        Vector2 desiredDir = CalculateDesiredDirection();

        currentDir = Vector2.Lerp(
            currentDir,
            desiredDir,
            turnRate * Time.fixedDeltaTime
        ).normalized;

        rb.velocity = currentDir * speed;
    }

    private Vector2 CalculateDesiredDirection()
    {
        Vector2 seekDir = ((Vector2)player.position - rb.position).normalized;

        Vector2 separationDir = CalculateSeparation();
        Vector2 avoidanceDir = CalculateAvoidance();

        return (seekDir + separationDir + avoidanceDir).normalized;
    }

    private Vector2 CalculateAvoidance()
    {
        Vector2 dir = rb.velocity.normalized;
        float distance = 1.2f;

        RaycastHit2D hit = Physics2D.Raycast(
            rb.position,
            dir,
            distance,
            obstacleMask
        );

        if (hit.collider != null)
        {
            Vector2 normal = hit.normal;

            Vector2 avoidDir = Vector2.Perpendicular(normal);

            if (Vector2.Dot(avoidDir, dir) < 0)
                avoidDir = -avoidDir;

            return avoidDir * 2f;
        }

        return Vector2.zero;
    }


    private Vector2 CalculateSeparation()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, separationRadius);
        Vector2 force = Vector2.zero;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Floter") && hit.gameObject != gameObject)
            {
                Vector2 diff = (Vector2)transform.position - (Vector2)hit.transform.position;
                force += diff.normalized / diff.magnitude;
            }
        }

        return force * separationForce;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.Instance.PlaySFX(hitAudioClip, collision.contacts[0].point, 1f);

        Vector2 refDir = Vector2.Reflect(currentDir, collision.contacts[0].normal);

        FXManager.Instance.Play(vfxHitKey, collision.contacts[0].point, Quaternion.FromToRotation(Vector3.up, refDir));

        TakeCollisionEfficiency(refDir);
    }
    private void TakeCollisionEfficiency(Vector2 direction)
    {
        currentDir = direction;
        TakeEfficiency(direction, hitEfficiencyForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = UnityEngine.Color.red;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }

    public void TakeEfficiency(Vector2 direction, float power)
    {
        if(isEfficiency)
            return;
        
        rb.AddForce(new Vector2(power * 0.8f * direction.x * knockbackMultiplier, power * 0.2f * knockbackMultiplier), ForceMode2D.Impulse);
        
        isEfficiency = true;
        
        StartCoroutine(TurnOffEfficiency(efficiencyTime));
    }

    IEnumerator TurnOffEfficiency(float time)
    {
        yield return new WaitForSeconds(time);
        isEfficiency = false;
    }

    public void OnPausedChanged(bool paused)
    {
        isPaused = paused;
    }
}
