using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : Weapon, IPointerClickHandler
{
    public UnityEvent OnDealt;
    public UnityEvent OnReshuffled;
    public UnityEvent OnDiscarded;

    [Header("References")]
    [SerializeField] CardData data = null;
    [SerializeField] List<Effect> effects = new List<Effect>();
    [HideInInspector] public CardManager cardManager = null;
    [HideInInspector] public Arcana arcana = null;

    bool _castable = true;
    bool _markedForDiscard = false;

    private void Update()
    {
        _castable = arcana.HasEnoughArcana(data.cost);
    }

    public override void Attack()
    {
        base.Attack();
        if (!_castable || !arcana.UseArcana(data.cost))
            return;
        Cast();
        //do the thing
        _markedForDiscard = true;
        cardManager.DeselectCard();
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
            yield return null;
        }
        StartCoroutine(AttemptCast());
    }

    public bool IsMarkedForDiscard()
    {
        return _markedForDiscard;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_castable)
            return;
        cardManager.SelectCard(this);
    }
}
