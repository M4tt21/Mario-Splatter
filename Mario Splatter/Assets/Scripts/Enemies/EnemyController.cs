using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health { get; private set; }

    public void initHealth(float health)
    {
        this.health = health;
    }

    public void death()
    {
        //morte
        Destroy(gameObject);
    }

    public void damage(float amount)
    {
        health -= amount;
    }
}
