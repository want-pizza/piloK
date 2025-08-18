using System.Collections;
using UnityEngine;

public class PlayerLifeCircle : MonoBehaviour
{
    [SerializeField] Vector3 respawnPoint = Vector3.zero;
    [SerializeField] Material irisMaterial;
    [SerializeField] float irisTime = 1f;
    [SerializeField] float irisSpeed = 2f;

    private Field<bool> isDead;
    public Field<bool> FieldIsDead => isDead;

    private void Awake()
    {
        isDead = new Field<bool>(false);
        Debug.Log($"IdleTransition отримав FieldIsDead hash={isDead.GetHashCode()}");
    }

    private void OnEnable()
    {
        isDead.OnValueChanged += OnIsDeadChanged;
    }

    private void OnDisable()
    {
        isDead.OnValueChanged -= OnIsDeadChanged;
    }

    private void OnIsDeadChanged(bool value)
    {
        Debug.Log($"{value} is dead");
        if (value) StartRespawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"collision.gameObject.layer = {collision.gameObject.layer}");
        if (collision.gameObject.layer == 8)
        {
            isDead.Value = true;
            Debug.Log("isDead = true");
        }
    }

    public void StartRespawn()
    {
        StartCoroutine(IrisRespawnRoutine());
    }

    public void UpdateRespawnPoint(Vector3 position)
    {
        respawnPoint = position;
    }

    private IEnumerator IrisRespawnRoutine()
    {
        irisMaterial.SetVector("_IrisCenter", transform.position);

        float darkness = 0f;
        while (darkness < 1f)
        {
            darkness += Time.deltaTime * irisSpeed;
            irisMaterial.SetFloat("_Darkness", Mathf.Clamp01(darkness));
            yield return null;
        }

        yield return new WaitForSeconds(irisTime);

        transform.position = respawnPoint;

        while (darkness > 0f)
        {
            darkness -= Time.deltaTime * irisSpeed;
            irisMaterial.SetFloat("_Darkness", Mathf.Clamp01(darkness));
            yield return null;
        }

        isDead.Value = false;
    }
}
