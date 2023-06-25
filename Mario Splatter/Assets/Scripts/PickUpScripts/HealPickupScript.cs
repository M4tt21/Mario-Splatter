using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPickupScript : PickupScript
{
    [Range(0f, 1f)]
    public float healPercentage = 0.4f;
    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<MarioHealth>().heal(healPercentage);
    }
}
