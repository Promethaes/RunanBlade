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

    private void Start() {
        _maxHP = health;
    }

    public float GetHealth()
    {
        return health;
    }
    public void TakeDamage(float damage, Vector3 knockback)
    {
        IEnumerator func()
        {
            void Dead() => deathDone = true;

            health -= damage;
            rigidbody.AddForce(knockback, ForceMode.Impulse);
            OnTakeDamage.Invoke();
            if (health <= 0)
            {
                OnDie.Invoke();

                yield return new WaitUntil(
                    () =>
                {
                    //this should be the last thing called in the listeners
                    OnDie.AddListener(Dead);
                    OnDie.Invoke();
                    return deathDone;//I finished my kill spree
                });

                OnDie.RemoveListener(Dead);
                if (disableOnDeath)
                    gameObject.SetActive(false);
            }
        }
        StartCoroutine(func());
    }

}
