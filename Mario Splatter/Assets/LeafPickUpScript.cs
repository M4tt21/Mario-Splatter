using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafPickUpScript : PickupScript
{
    protected override void pickUpAction(GameObject player)
    {

        player.GetComponent<PlayerController>().activateInfiniteStamina();
    }
}
