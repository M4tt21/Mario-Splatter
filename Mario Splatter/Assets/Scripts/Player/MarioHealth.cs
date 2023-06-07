using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    void Start()
    {
        fullHealth();
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
        currentShield = math.clamp((maxShield * percentageAmount)+currentShield, 0, maxShield);
        
    }

    public void fullHealth()
    {
        currentHealth = maxHealth;
    }
}
