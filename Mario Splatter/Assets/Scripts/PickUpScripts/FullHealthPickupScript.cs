using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullHealthPickupScript : PickupScript
{
    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<MarioHealth>().fullHealth();
    }
}
