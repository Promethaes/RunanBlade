using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [HideInInspector] public bool finishedCasting = false;
    [HideInInspector] public GameObject ownerEntity = null;
    public int charges = 1;

    int _maxCharges = 0;
    private void Awake()
    {
        _maxCharges = charges;
    }
    public virtual void Cast()
    {
        charges--;
        finishedCasting = true;
    }

    public virtual void ResetEffect()
    {
        charges = _maxCharges;
    }
}
