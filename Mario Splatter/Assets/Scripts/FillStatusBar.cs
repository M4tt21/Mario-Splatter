using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillStatusBar : MonoBehaviour
{
    public Slider slider;
    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        float fillValue = MarioHealth.Instance.currentHealth / MarioHealth.Instance.maxHealth;
        slider.value = fillValue;
    }
}
