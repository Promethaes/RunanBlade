using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDoer : MonoBehaviour
{
    [SerializeField] float dmg = 0.0f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var hp = other.gameObject.GetComponent<Health>();
        if (!hp)
            return;
        hp.TakeDamage(dmg);
    }
}
