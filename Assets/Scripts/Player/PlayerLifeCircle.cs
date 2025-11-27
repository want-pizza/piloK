using System.Collections;
using Unity.VisualScripting;
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
        GetComponent<PlayerStats>().CurrentHealth.OnValueChanged += OnCurrentHpChanged;
    }

    private void OnDisable()
    {
        isDead.OnValueChanged -= OnIsDeadChanged;
        GetComponent<PlayerStats>().CurrentHealth.OnValueChanged -= OnCurrentHpChanged;
    }
    private void OnCurrentHpChanged(float value) => isDead.Value = value <= 0f ? true : false;
    private void OnIsDeadChanged(bool value)
    {
        Debug.Log($"{value} is dead");
    }
    public void TeleportToRespawnPoint()
    {
        transform.position = respawnPoint;
    }
    public void Respawn()
    {
        //Debug.Log(" isDead.Value = false;");
        isDead.Value = false;
        PlayerStats playerStats = GetComponent<PlayerStats>();
        playerStats.CurrentHealth.Value = playerStats.MaxHealth.Value;
    }
}
