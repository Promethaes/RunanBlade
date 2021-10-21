using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    [SerializeField] protected float health;
    [SerializeField] bool disableOnDeath = true;

    public UnityEvent OnTakeDamage, OnDie;
    private bool deathDone = false;
    protected float _maxHP = 0.0f;

    [Header("Reference")]
    [SerializeField] new Rigidbody rigidbody = null;

    private void Start()
    {
        _maxHP = health;
    }

    public float GetHealth()
    {
        return health;
    }
    public virtual void TakeDamage(float damage, Vector3 knockback)
    {
        health -= damage;
        if (knockback.magnitude != 0.0f)
            rigidbody.AddForce(knockback, ForceMode.Impulse);
        OnTakeDamage.Invoke();
        if (health <= 0)
            OnDie.Invoke();
    }

}
