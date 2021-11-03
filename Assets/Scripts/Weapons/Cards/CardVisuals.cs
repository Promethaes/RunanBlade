using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardVisuals : MonoBehaviour
{
    [SerializeField] Color selectedColour = Color.green;
    [Tooltip("False: back facing. True: front facing.")]
    public bool flipped = false;
    [Header("References")]
    [SerializeField] GameObject Back = null;
    [SerializeField] GameObject Front = null;
    [SerializeField] Card card = null;
    [SerializeField] Image cardBackground = null;


    [HideInInspector] public Transform cardSlot = null;

    Color _originalColour = Color.black;
    // Start is called before the first frame update
    void Start()
    {
        void OnDeal()
        {
            flipped = true;
            Back.SetActive(false);
            Front.SetActive(true);
        }

        void OnReshuffle()
        {
            flipped = false;
            Front.SetActive(false);
            Back.SetActive(true);
        }

        void OnSelect()
        {
            cardBackground.color = selectedColour;
        }
        void OnDeselect()
        {
            cardBackground.color = _originalColour;
        }

        _originalColour = cardBackground.color;

        card.OnDealt.AddListener(OnDeal);
        card.OnReshuffled.AddListener(OnReshuffle);
        card.OnSelected.AddListener(OnSelect);
        card.OnDeselected.AddListener(OnDeselect);
    }

}
