using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour
{
    public enum hitType { head, body, leg, arm }
    public hitType ht;
    public EnemyController controller;

    public void Hit(int amount)
    {
        controller.damage(amount);
    }
}
