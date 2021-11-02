using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Card : Weapon
{
    public UnityEvent OnThisCardDealt;
    public UnityEvent OnThisCardReshuffled;
    [Header("References")]
    [SerializeField] CardData data = null;
    [SerializeField] List<Effect> effects = new List<Effect>();
    [HideInInspector] public Arcana arcana = null;

    bool _castable = true;
    bool _markedForDiscard = false;

    private void Start()
    {
    }

    public override void Attack()
    {
        if (!_castable || !arcana.UseArcana(data.cost))
            return;
        Cast();
        //do the thing
        _castable = false;
        _markedForDiscard = true;
    }
    public void Cast()
    {
        IEnumerator AttemptCast()
        {
            foreach (var e in effects)
            {
                e.Cast();
                while (!e.finishedCasting)
                    yield return null;
            }
        }
        StartCoroutine(AttemptCast());
    }

    public bool IsMarkedForDiscard()
    {
        return _markedForDiscard;
    }
}
