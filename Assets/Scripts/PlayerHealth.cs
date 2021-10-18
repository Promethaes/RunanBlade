using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{

    Vector3 _spawnPos = Vector3.zero;
    private void OnEnable()
    {
        _spawnPos = transform.position;
        void Die()
        {
            transform.position = _spawnPos;
            health = _maxHP;
        }
        OnDie.AddListener(Die);
    }
}
