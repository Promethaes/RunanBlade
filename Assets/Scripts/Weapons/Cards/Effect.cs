using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    [HideInInspector] public bool finishedCasting = false;
    public virtual void Cast()
    {
        //example
        finishedCasting = true;
    }
}
