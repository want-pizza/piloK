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
        //Debug.Log($"IdleTransition get FieldIsDead hash={isDead.GetHashCode()}");
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log($"collision.gameObject.layer = {collision.gameObject.layer}");
        if (collision.gameObject.layer == 8)
        {
            isDead.Value = true;
            //Debug.Log("isDead = true");
        }
    }
    public void TeleportToRespawnPoint()
    {
        transform.position = respawnPoint;
    }
    public void Respawn()
    {
        //Debug.Log(" isDead.Value = false;");
        isDead.Value = false;
    }
}
