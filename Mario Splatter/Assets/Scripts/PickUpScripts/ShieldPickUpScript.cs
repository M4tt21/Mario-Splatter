using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPickUpScript : PickupScript
{
    [Header("Amount of Shield in Percentage")]
    [Range(0,1f)]
    public float amount;
    // Start is called before the first frame update
    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<MarioHealth>().addShield(amount);
    }
}
