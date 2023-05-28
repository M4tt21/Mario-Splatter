using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Default Stats gun
    public virtual float reloadTime { protected set; get; } = 1f ;
    public float shotCD { protected set; get; } = 0.1f;
    public int magazineSize { protected set; get; } = 24;

    public int currentAmmo;

    public bool isOutOfAmmo;

    //CD states
    public bool isOnCooldown { protected set; get; } = false;
    public bool isReloading { protected set; get; } = false;


    
    public virtual bool fire(GameObject camera)
    {
        Debug.Log("This gun is WiP.");
        return false;
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
        currentAmmo = magazineSize;
        isOutOfAmmo = false;
    }
}
