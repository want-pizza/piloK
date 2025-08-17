using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Field<bool> isDead;
    public Field<bool> FieldIsDead => isDead;
    private void Awake()
    {
        isDead = new Field<bool>(false);
    }
    private void OnEnable()
    {
        isDead.OnValueChanged += OnIsDeadChanged;
        Debug.Log($"Передаємо FieldIsDead: {FieldIsDead.GetHashCode()}");
    }
    private void OnDisable()
    {
        isDead.OnValueChanged -= OnIsDeadChanged;
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
    private void OnIsDeadChanged(bool value)
    {
        Debug.Log($"{value} is dead");
    }
}
