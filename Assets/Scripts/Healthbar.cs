using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Slider slider = null;

    [Header("References")]
    [SerializeField] Health health = null;

    float _startingHealth = 0.0f;

    private void Start()
    {
        _startingHealth = health.GetHealth();
    }

    private void Update()
    {
        slider.value = health.GetHealth() / _startingHealth;
    }

}
