using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "CardData", menuName = "CardData", order = 1)]
public class CardData : ScriptableObject
{
    public string cardName;
    public int cost;
    public string tooltip;
    public Image cardImage;
}