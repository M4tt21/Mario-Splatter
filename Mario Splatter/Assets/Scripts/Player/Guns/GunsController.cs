using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsController : MonoBehaviour
{
    private bool isFireEnabled = true;
    public bool isReEquipping { private set; get; } = false;
    public bool isReloading = false;
    public bool isCurrentGunOutOfAmmo = false;
    private float ReEquipCD = 0.5f;
    [SerializeField]
    private CanvasScript cs;
    public enum gunType { AR, SG, P }

    public static gunType startingGun = gunType.P;
    private gunType currentGun;

    public gunType GetCurrentGun() => currentGun;
    private void SetCurrentGun(gunType value){currentGun = value;}

    private int ignoreLayerMask;

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
    private bool isARUnlocked = false;
    [SerializeField]
    private bool isSGUnlocked = false;
    [SerializeField]
    private bool isPUnlocked = false;

    private GameObject DefaultGun;
    KoopaHit kh;

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

    private void Update()
    {
        
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
        if (isFireEnabled && isGunUnlocked(currentGun))//tasto sinistro mouse
        {
            tryGetGunObjFromType(currentGun).GetComponent<Gun>().fire(camera);
            isCurrentGunOutOfAmmo = tryGetGunObjFromType(currentGun).GetComponent<Gun>().isOutOfAmmo;
        }
    }

    private GameObject tryGetGunObjFromType(gunType gunT)
    {
        if (GunsData.TryGetValue(gunT, out GameObject result))
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
                cs.updateCrossAir(gun);
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
        isFireEnabled = true;
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
        isFireEnabled = false;
    }

    public IEnumerator reloadTime()
    {
        isReloading = true;

        yield return new WaitForSeconds(tryGetGunObjFromType(currentGun).GetComponent<Gun>().reloadTime);

        tryGetGunObjFromType(currentGun).GetComponent<Gun>().reload();
        isCurrentGunOutOfAmmo = false;
        isReloading = false;
    }

    public void reloadCurrentGun()
    {
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

}





