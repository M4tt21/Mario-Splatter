using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioHealth : MonoBehaviour
{
    [Header("Mario Health Stats")]
    public float maxHealth = 10f;
    [Range(0f, 10f)]
    public float currentHealth = 0;
    public float maxShield = 50f;
    [Range(0f, 50f)]
    public float currentShield = 0;
    public float maxStamina = 100f;
    [Range(0f, 100f)]
    public float currentStamina = 0;
    public bool isStaminaConsuming = false;
    public float staminaConsumeSpeed = 1f;
    public float staminaReplenishSpeed = 1f;

    void Start()
    {
        fullHealth();
        fullStamina();
    }

    private void FixedUpdate()
    {
        consumeStamina(isStaminaConsuming);
    }


    //If value is true then the stamina is being consumed, else it's being replanished
    public float consumeStamina(bool consume)
    {
        if(consume)//Consume
            currentStamina = Mathf.Clamp(currentStamina - (staminaConsumeSpeed), 0f, maxStamina);
        else
            currentStamina = Mathf.Clamp(currentStamina + (staminaReplenishSpeed), 0f, maxStamina);
        return currentStamina;
    }

    public float TakeDamage(float amount)
    {
        currentShield -= amount;
        if (currentShield < 0)
        {
            currentHealth += currentShield;
            currentShield = 0;
        }
        return currentHealth;
    }

    public void addShield(float percentageAmount)
    {
        currentShield = Mathf.Clamp((maxShield * percentageAmount)+currentShield, 0, maxShield);
        
    }

    public void fullHealth()
    {
        currentHealth = maxHealth;
    }
    public void fullStamina()
    {
        currentStamina = maxStamina;
    }
}
