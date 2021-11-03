﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardManager : MonoBehaviour
{
    [System.Serializable]
    public class DeckHelper
    {
        public Card card;
        public int numCopies;
    };
    [SerializeField] List<DeckHelper> deckHelpers = new List<DeckHelper>();
    [SerializeField] AnimationCurve lerpCurve = null;

    [Tooltip("How fast cards are drawn.")]
    [SerializeField] float dealTime = 0.20f;

    [Tooltip("How fast cards move to their slots.")]
    [SerializeField] float dealSpeed = 1.0f;
    public UnityEvent OnDeal;
    public UnityEvent OnCardDealt;


    [Header("References")]
    [SerializeField] List<Transform> cardSlots = new List<Transform>();
    [SerializeField] Transform discardPile = null;
    [SerializeField] Arcana arcana = null;
    [SerializeField] WeaponManager weaponManager = null;
    List<Card> _deck = new List<Card>();
    List<Card> _hand = new List<Card>();
    List<Card> _discard = new List<Card>();

    //false for free, true for occupied
    Dictionary<Transform, bool> _internalCardSlots = new Dictionary<Transform, bool>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var dh in deckHelpers)
            for (int i = 0; i < dh.numCopies; i++)
            {
                _deck.Add(GameObject.Instantiate(dh.card, transform.position, Quaternion.identity, transform));
                _deck[_deck.Count - 1].arcana = arcana;
                _deck[_deck.Count - 1].cardManager = this;
            }
        foreach (var c in cardSlots)
            _internalCardSlots.Add(c, false);

        IEnumerator DealInitialHand()
        {
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < 5; i++)
            {
                Deal();
                yield return new WaitForSeconds(dealTime);
            }
        }
        StartCoroutine(DealInitialHand());
    }

    private void Update()
    {
        for (int i = 0; i < _hand.Count; i++)
        {
            if (_hand[i].IsMarkedForDiscard())
            {
                Discard(_hand[i]);
                i--;
            }
        }

    }

    void Deal()
    {
        for (int i = cardSlots.Count - 1; i >= 0; i--)
        {
            if (_internalCardSlots[cardSlots[i]])
                continue;

            var card = _deck[_deck.Count - 1];
            _deck.Remove(card);
            _hand.Add(card);
            weaponManager.AddWeapon(card);
            _internalCardSlots[cardSlots[i]] = true;
            OnDeal.Invoke();
            card.OnDealt.Invoke();

            IEnumerator Lerp()
            {
                float x = 0.0f;
                while (x < 1.0f)
                {
                    yield return new WaitForEndOfFrame();
                    x += Time.deltaTime * dealSpeed;
                    card.transform.position = Vector3.Lerp(transform.position, cardSlots[i].transform.position, lerpCurve.Evaluate(x));
                }
                OnCardDealt.Invoke();
                card.GetComponent<CardVisuals>().cardSlot = cardSlots[i];
                card.transform.SetParent(cardSlots[i]);
            }
            StartCoroutine(Lerp());
            return;
        }
    }

    void Discard(Card card)
    {
        _hand.Remove(card);
        _discard.Add(card);
        weaponManager.RemoveWeapon(card);
        IEnumerator Lerp()
        {
            var cardSlot = card.GetComponent<CardVisuals>().cardSlot;
            card.transform.SetParent(discardPile);
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;
                card.transform.position = Vector3.Lerp(cardSlot.transform.position, discardPile.transform.position, lerpCurve.Evaluate(x));
            }
            card.GetComponent<CardVisuals>().cardSlot = null;
        }
        StartCoroutine(Lerp());
    }

    public void SelectCard(Card c)
    {
        weaponManager.ChangeWeapon(c);
    }
    public void DeselectCard()
    {
        weaponManager.ChangeWeapon(null);
    }

}
