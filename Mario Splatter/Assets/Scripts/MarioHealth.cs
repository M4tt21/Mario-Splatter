using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarioHealth : MonoBehaviour
{
    public float maxHealth { get; private set; } = 10;
    public float currentHealth { get; private set; } = 0;
    public static MarioHealth Instance;
    private void Awake() => Instance = this;
    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        
    }
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if(currentHealth <=0)
        {
            // siamo morti
            //animazione morte
        }
    }

}
