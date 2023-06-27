using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsController : MonoBehaviour
{
    public bool isCurrentGunEnabled = true;
    public bool isReEquipping = false;
    public bool isReloading = false;
    public bool isCurrentGunOutOfAmmo = false;
    private float ReEquipCD = 0.5f;
    public enum gunType { AR, SG, P }

    public static gunType startingGun = gunType.P;
    private gunType currentGun;

    public gunType GetCurrentGun() => currentGun;
    private void SetCurrentGun(gunType value){currentGun = value;}

    /*Guns Cooldowns*/
    public static float ARCooldown = 0.08f;
    public static float SGCooldown = 0.01f;
    public static float PCooldown = 0.01f;

    [SerializeField]
    private GameObject camera;
    
    //Gun Objects
    [SerializeField]
    private GameObject rifleObj;
    [SerializeField]
    private GameObject shotgunObj;
    [SerializeField]
    private GameObject pistolObj;

    [Header("Unlocked Guns")]
    [SerializeField]
    public bool isARUnlocked = false;
    [SerializeField]
    public bool isSGUnlocked = false;
    [SerializeField]
    public bool isPUnlocked = false;

    private GameObject DefaultGun;

    //Gun data
    private Dictionary<gunType, GameObject> GunsData;


    void Start()
    {
        //Starting Guns
        currentGun = startingGun;
        unlockGun(startingGun);
        GunsData = new Dictionary<gunType, GameObject>();
        GunsData.Add(gunType.AR, rifleObj);
        GunsData.Add(gunType.SG, shotgunObj);
        GunsData.Add(gunType.P, pistolObj);

        enableCurrentGun();
        selectGun(currentGun);

        DefaultGun = pistolObj;
    }

    public bool isGunUnlocked(gunType gun)
    {
        switch (gun){
            case gunType.AR:
                return isARUnlocked;
            case gunType.SG:
                return isSGUnlocked;
            case gunType.P:
                return isPUnlocked;
            default:
                return false;
        }
    }
    
    public void unlockGun(gunType gun)
    {
        switch (gun)
        {
            case gunType.AR:
                isARUnlocked=true;
                break;
            case gunType.SG:
                isSGUnlocked=true;
                break;
            case gunType.P:
                isPUnlocked=true;
                break;
        }
    }

    public void fireCurrentGun()
    {
        Debug.DrawRay(camera.transform.position, camera.transform.forward * 1000, Color.red); // traiettoria proiettile visibile
        if (isCurrentGunEnabled && isGunUnlocked(currentGun))//tasto sinistro mouse
        {
            tryGetGunObjFromType(currentGun).GetComponent<Gun>().fire(camera);
            isCurrentGunOutOfAmmo = tryGetGunObjFromType(currentGun).GetComponent<Gun>().isOutOfAmmo;
        }
    }

    private GameObject tryGetGunObjFromType(gunType gunT)
    {
        if (GunsData!=null && GunsData.TryGetValue(gunT, out GameObject result))
            return result;
        else return DefaultGun;
    }

    public void selectGun(gunType gun)
    {
        if (currentGun != gun && isGunUnlocked(gun))
        {
            if(GunsData.TryGetValue(gun, out GameObject newGunObj) && GunsData.TryGetValue(currentGun, out GameObject oldGunObj))
            {
                oldGunObj.GetComponent<Gun>().setVisible(false);
                newGunObj.GetComponent<Gun>().setVisible(true);

                //Cambio Mirino
                
            }
            currentGun = gun;
        }
    }

    public void hideAll(bool value)
    {
        foreach(KeyValuePair<gunType, GameObject> gun in GunsData)
        {
            gun.Value.GetComponent<Gun>().setVisible(value);
        }
    }

    private IEnumerator ReEquipTime()
    {
        isReEquipping = true;

        yield return new WaitForSeconds(ReEquipCD);

        tryGetGunObjFromType(currentGun).GetComponent<Gun>().setVisible(true);
        isCurrentGunEnabled = true;
        isReEquipping = false;
    }

    public void enableCurrentGun()
    {   
        if (isReEquipping)
            return;
        StartCoroutine(ReEquipTime());
    }

    public void disableCurrentGun()
    {
        tryGetGunObjFromType(currentGun).GetComponent<Gun>().setVisible(false);
        isCurrentGunEnabled = false;
    }

    public IEnumerator reloadTime()
    {
        if (tryGetGunObjFromType(currentGun).GetComponent<Gun>().ammoHeld <= 0)
            yield break;
        isReloading = true;

        Gun gunScript = tryGetGunObjFromType(currentGun).GetComponent<Gun>();

        gunScript.playReloadSound();
        yield return new WaitForSeconds(gunScript.reloadSound.length);

        gunScript.reload();
        isCurrentGunOutOfAmmo = false;
        isReloading = false;
    }

    public void reloadCurrentGun()
    {
        Debug.Log("" + currentGun);
        if (isReloading)
            return;
        StartCoroutine(reloadTime());
    }

    public int getAmmoOfCurrentGun()
    {
        return tryGetGunObjFromType(currentGun).GetComponent<Gun>().currentAmmo;
    }
    public int getMaxAmmoOfCurrentGun()
    {
        return tryGetGunObjFromType(currentGun).GetComponent<Gun>().magazineSize;
    }

    public int getAmmoHeldOfCurrentGun()
    {
        return tryGetGunObjFromType(currentGun).GetComponent<Gun>().ammoHeld;
    }

    public void addAmmoToGun(gunType gun, int amount)
    {
        tryGetGunObjFromType(gun).GetComponent<Gun>().addAmmo(amount);
    }

    public int getAmmoHeldOfGun(gunType gun)
    {
        return tryGetGunObjFromType(gun).GetComponent<Gun>().ammoHeld;
    }

    public int getAmmoInGun(gunType gun)
    {
        return tryGetGunObjFromType(gun).GetComponent<Gun>().currentAmmo;
    }

    public void setAmmoHeldOfGun(gunType gun, int ammount)
    {
        tryGetGunObjFromType(gun).GetComponent<Gun>().ammoHeld = ammount;
    }

    public void setAmmoInGun(gunType gun, int ammount)
    {
        tryGetGunObjFromType(gun).GetComponent<Gun>().currentAmmo = ammount;
    }

    


}





