using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]

    [SerializeField]public float health = 300;
    [SerializeField]public float headHitMul = 2f;
    [SerializeField]public float bodyHitMul = 1.2f;
    [SerializeField]public float legHitMul = 1f;
    [SerializeField] public float armHitMul = 1f;

    public void initHealth(float health)
    {
        this.health = health;
    }

    public void death()
    {
        //morte
        Destroy(gameObject);
    }

    public void damage(float ammount, EnemyHit.hitType ht)
    {
        switch (ht){
            case EnemyHit.hitType.head:
                health -= ammount * headHitMul;
                break;
            case EnemyHit.hitType.body:
                health -= ammount * bodyHitMul;
                break;
            case EnemyHit.hitType.leg:
                health -= ammount * legHitMul;
                break;
            case EnemyHit.hitType.arm:
                health -= ammount * armHitMul;
                break;
        }
    }
}
