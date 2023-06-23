using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Default Stats gun
    [Header("General Gun Stats")]
    [SerializeField]public float reloadTime = 1f ;
    [SerializeField]public float range = Mathf.Infinity;
    [SerializeField]public float shotCD = 0.1f;
    [SerializeField]public int magazineSize = 24;
    [SerializeField]public int ammoHeld=24;
    [SerializeField]public int bulletDMG = 50;

    public int currentAmmo;

    public bool isOutOfAmmo;
    [SerializeField] public GameObject gunFX;
    protected ParticleSystem ps;

    //CD states
    public bool isOnCooldown = false;
    public bool isReloading = false;

    public virtual void fire(GameObject camera)
    {
        Debug.Log("This gun is WiP.");
    }
    protected IEnumerator gunCooldownTime()
    {
        isOnCooldown = true;
        
        yield return new WaitForSeconds(shotCD);
        
        isOnCooldown = false;
    }

    public void setVisible(bool value)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = value;
    }

    public void reload()
    {
        int ammoRequested = magazineSize - currentAmmo;
        if (ammoRequested > ammoHeld) 
        {
            currentAmmo += ammoHeld;
            ammoHeld = 0;
        }
        else
        {
            currentAmmo += ammoRequested;
            ammoHeld -= ammoRequested;
        }
            
        isOutOfAmmo = false;
    }

    public void addAmmo(int amount)
    {
        ammoHeld += amount;
    }

    public void playFX(Vector3 position, Vector3 forwardDirection)
    {
        if (gunFX != null)
        {
            GameObject currentFX = Instantiate(gunFX);
            currentFX.transform.position = position;
            currentFX.transform.forward = forwardDirection;
            StartCoroutine(despawnFX(currentFX));


        }
        else
            Debug.Log("No FX Found.");
    }

    IEnumerator despawnFX(GameObject fx)
    {
        yield return new WaitForSeconds(1);
        Destroy(fx);
    }
}
