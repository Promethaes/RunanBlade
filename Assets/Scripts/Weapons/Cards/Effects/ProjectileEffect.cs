using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEffect : Effect
{
    [SerializeField] float speed = 1.0f;
    [Header("References")]
    [SerializeField] Projectile projectile = null;
    SpellStartPos _spellStartPos = null;


    private void Start()
    {
    }
    //Projectile Effect
    //Multi projectile effect will just have a nested series of projectile effects
    public override void Cast()
    {
        _spellStartPos = ownerEntity.GetComponent<PlayerSpellStartPos>();
        var dir = (_spellStartPos.startPos.position - ownerEntity.transform.position).normalized;
        var p = GameObject.Instantiate(projectile);
        p.transform.position = _spellStartPos.startPos.transform.position;
        p.rigidbody2D.AddForce(dir * speed, ForceMode2D.Impulse);
        base.Cast();
    }
}
