using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickupScript : MonoBehaviour
{

    [Header("Weapon Type")]
    [SerializeField]
    public GunsController.gunType gun;
    [Header("Amount of Ammo")]
    [SerializeField]
    public int amount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<GunsController>().addAmmoToGun(gun, amount);
        }
    }
}
