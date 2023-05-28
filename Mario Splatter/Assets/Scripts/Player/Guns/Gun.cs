using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Default Stats gun
    [Header("General Stats")]
    [SerializeField]public float reloadTime = 1f ;
    [SerializeField]public float range = Mathf.Infinity;
    [SerializeField]public float shotCD = 0.1f;
    [SerializeField]public int magazineSize = 24;
    [SerializeField]public int bulletDMG = 50;

    public int currentAmmo;

    public bool isOutOfAmmo;

    //CD states
    public bool isOnCooldown { protected set; get; } = false;
    public bool isReloading { protected set; get; } = false;


    
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
        currentAmmo = magazineSize;
        isOutOfAmmo = false;
    }
}
