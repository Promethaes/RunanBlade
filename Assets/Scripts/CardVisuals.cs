using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisuals : MonoBehaviour
{
    [Tooltip("False: back facing. True: front facing.")]
    public bool flipped = false;
    [Header("References")]
    [SerializeField] GameObject Back = null;
    [SerializeField] GameObject Front = null;
    [SerializeField] Card card = null;

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

        card.OnThisCardDealt.AddListener(OnDeal);
    }

}
