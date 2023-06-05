using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickupScript : PickupScript
{

    [Header("Weapon Type Unlock")]
    [SerializeField]
    public GunsController.gunType gun;
    // Start is called before the first frame update
    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<GunsController>().unlockGun(gun);
    }
}
