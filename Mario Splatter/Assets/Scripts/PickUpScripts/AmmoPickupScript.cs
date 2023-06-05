using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupScript : PickupScript
{

    [Header("Weapon Type")]
    [SerializeField]
    public GunsController.gunType gun;
    [Header("Amount of Ammo")]
    [SerializeField]
    public int amount;

    protected override void pickUpAction(GameObject player)
    {
        player.GetComponent<GunsController>().addAmmoToGun(gun, amount);
    }
}
