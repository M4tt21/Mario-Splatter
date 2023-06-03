using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarsScript : MonoBehaviour
{
    [Header("References Needed")]
    public MarioHealth marioHealth;
    private Slider sliderHealth;
    private Slider sliderShield;
    void Start()
    {
        sliderHealth = transform.Find("SliderHealth").GetComponent<Slider>();
        sliderShield = transform.Find("SliderShield").GetComponent<Slider>();
    }

    void FixedUpdate()
    {
        Debug.Log(marioHealth.currentHealth + " " + marioHealth.maxHealth);
        float fillHealthValue = marioHealth.currentHealth / marioHealth.maxHealth;
        sliderHealth.value = fillHealthValue;
        float fillShieldValue = marioHealth.currentShield / marioHealth.maxShield;
        sliderShield.value = fillShieldValue;
    }
}
