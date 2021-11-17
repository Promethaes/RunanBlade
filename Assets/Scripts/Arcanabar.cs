using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arcanabar : MonoBehaviour
{
    [SerializeField] Slider slider = null;

    [Header("References")]
    [SerializeField] Arcana arcana = null;

    float _startingArcana = 0.0f;

    private void Start()
    {
        _startingArcana = arcana.GetArcana();
    }

    private void Update()
    {
        slider.value = arcana.GetArcana() / _startingArcana;
    }
}
