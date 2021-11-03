using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [Tooltip("-1 for infinite")]
    [SerializeField] protected int ammo = -1;
    [HideInInspector] public bool attacking = false;

    public UnityEvent OnAttack;
    public UnityEvent OnSelected;
    public UnityEvent OnDeselected;

    public int GetAmmo()
    {
        return ammo;
    }

    public virtual void Attack()
    {
        if (ammo != -1)
            ammo--;
        OnAttack.Invoke();
    }

}
