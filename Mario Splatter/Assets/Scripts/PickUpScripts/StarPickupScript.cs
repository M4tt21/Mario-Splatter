using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPickupScript : PickupScript
{


    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<PlayerController>().starNextlevel();
        player.GetComponent<PlayerController>().isImmune = true;
    }
}
