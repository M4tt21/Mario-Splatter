using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    
    public enum hitType { head, body, leg, arm }
    [SerializeField]
    public hitType ht = hitType.body;

    [SerializeField]
    public float damage = 10f; //Default
    public EnemyController controller;

    public virtual void Hit(float damage)
    {
        controller.damage(damage, ht); //Default
    }
}
