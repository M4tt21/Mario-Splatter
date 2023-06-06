using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarsScript : MonoBehaviour
{
    private Slider sliderHealth;
    private Slider sliderShield;
    void Start()
    {
        sliderHealth = transform.Find("SliderHealth").GetComponent<Slider>();
        sliderShield = transform.Find("SliderShield").GetComponent<Slider>();
        
    }
    public void updateStatus(MarioHealth marioHealth)
    {
        float fillHealthValue = marioHealth.currentHealth / marioHealth.maxHealth;
        sliderHealth.value = fillHealthValue;
        float fillShieldValue = marioHealth.currentShield / marioHealth.maxShield;
        sliderShield.value = fillShieldValue;
    }
}
