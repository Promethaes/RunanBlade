using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.Events;

public class Health : MonoBehaviour
{

    [SerializeField] protected float health = 0.0f;

    public UnityEvent OnTakeDamage, OnDie;
    protected float _maxHP = 0.0f;

    private void Start()
    {
        _maxHP = health;
    }

    public float GetHealth()
    {
        return health;
    }
    //knockback should be done on the attacker's side
    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        OnTakeDamage.Invoke();
        if (health <= 0)
            OnDie.Invoke();
    }

}
