using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Field<bool> isDead = new Field<bool>(false);
    public Field<bool> FieldIsDead => isDead;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"collision.gameObject.layer = {collision.gameObject.layer}");
        if (collision.gameObject.layer == 8)
        {
            isDead.Value = true;
        }
    }
}
