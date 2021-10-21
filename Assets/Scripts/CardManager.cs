using System.Collections;
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
    [SerializeField] float dealSpeed = 1.0f;
    public UnityEvent OnDeal;
    public UnityEvent OnCardDealt;
    public bool dealFiveCards = false;


    [Header("References")]
    [SerializeField] List<Transform> cardSlots = new List<Transform>();
    [SerializeField] Transform discardPile = null;
    List<Card> _deck = new List<Card>();
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
                _deck[_deck.Count - 1].GetComponent<Card>().cardManager = this;
            }
        foreach (var c in cardSlots)
            _internalCardSlots.Add(c, false);
    }


    private void Update()
    {
        if (dealFiveCards)
        {
            dealFiveCards = false;
            IEnumerator Dealout()
            {
                for (int i = 0; i < 5; i++)
                {
                    Deal();
                    yield return new WaitForSeconds(0.20f);
                }
            }
            StartCoroutine(Dealout());
        }
    }

    public void Deal()
    {
        for (int i = cardSlots.Count - 1; i >= 0; i--)
        {
            if (_internalCardSlots[cardSlots[i]])
                continue;

            var card = _deck[_deck.Count - 1];
            _deck.Remove(card);
            _internalCardSlots[cardSlots[i]] = true;
            OnDeal.Invoke();
            card.OnThisCardDealt.Invoke();

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
                card.cardSlot = cardSlots[i];
                card.transform.SetParent(cardSlots[i]);
            }
            StartCoroutine(Lerp());
            return;
        }
    }

    public void Discard(Card card)
    {
        _discard.Add(card);
        IEnumerator Lerp()
        {
            float x = 0.0f;
            while (x < 1.0f)
            {
                yield return new WaitForEndOfFrame();
                x += Time.deltaTime;
                card.transform.position = Vector3.Lerp(card.cardSlot.transform.position, discardPile.transform.position, lerpCurve.Evaluate(x));
            }
            card.cardSlot = null;
            card.transform.SetParent(discardPile);
        }
        StartCoroutine(Lerp());
    }
}
