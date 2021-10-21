using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : MonoBehaviour
{
    public UnityEvent OnThisCardDealt;
    [SerializeField] bool cast = false;
    [Header("References")]
    [SerializeField] CardData data = null;
    [HideInInspector] public CardManager cardManager = null;
    [HideInInspector] public Transform cardSlot = null;

    bool _castable = true;

    private void Update()
    {
        if (cast)
        {
            cast = false;
            Cast();
        }
    }

    public virtual void Cast()
    {
        if (!_castable)
            return;
        //do the thing
        _castable = false;
        cardManager.Discard(this);
    }
}
