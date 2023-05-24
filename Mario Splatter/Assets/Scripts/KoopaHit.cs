using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaHit : MonoBehaviour
{
    public enum hitType { head,body}
    public hitType ht;
    public KoopaController controller;

    public void Hit(int amount)
    {
        controller.health -= amount;
        Debug.Log("vita:" + controller.health);
    }
    public void porcodio()
    {
        Debug.Log("caccaaa");
    }
}
