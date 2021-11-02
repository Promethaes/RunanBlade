using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Arcana : MonoBehaviour
{
    [SerializeField] protected float arcana = 0.0f;
    public UnityEvent OnUseArcana;
    public UnityEvent OnArcanaEmpty;

    protected float _maxArcana = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        _maxArcana = arcana;
    }

    public float GetArcana()
    {
        return arcana;
    }

    public bool UseArcana(float cost)
    {
        if(arcana - cost < 0.0f)
            return false;
        arcana -= cost;
        OnUseArcana.Invoke();
        if (arcana <= 0.0f)
        {
            arcana = 0.0f;
            OnArcanaEmpty.Invoke();
        }
        return true;
    }

}
