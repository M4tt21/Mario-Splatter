using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioHealth : MonoBehaviour
{
    [Header("Mario Health Stats")]
    public float maxHealth = 10f;
    [Range(0f, 10f)]
    public float currentHealth = 0;
    public float maxShield = 5f;
    [Range(0f, 5f)]
    public float currentShield = 0;
    void Start()
    {
        fullHealth();
    }

    void Update()
    {
        
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

    public void fullHealth()
    {
        currentHealth = maxHealth;
    }
}
