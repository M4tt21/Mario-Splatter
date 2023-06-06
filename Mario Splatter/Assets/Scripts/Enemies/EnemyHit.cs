using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    
    public enum hitType { head, body, leg, arm }
    public hitType ht = hitType.body;
    public EnemyController controller;

    public virtual void Hit(float damage)
    {
        if (controller != null) controller.damage(damage, ht); //Default
    }
}
