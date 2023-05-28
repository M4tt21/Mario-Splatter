using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunsController : MonoBehaviour
{
    private bool isFireEnabled = true;
    private bool isReEquipping = false;
    private float ReEquipCD = 0.5f;
    [SerializeField]
    private CanvasScript cs;
    public enum gunType { AR, SG, P }

    public static gunType startingGun = gunType.AR;
    private gunType currentGun;

    public gunType GetCurrentGun() => currentGun;
    private void SetCurrentGun(gunType value){currentGun = value;}

    private int ignoreLayerMask;

    /*Guns Cooldowns*/
    private bool isOnCooldown=false;
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

    private GameObject DefaultGun;
    KoopaHit kh;

    //Gun data
    private Dictionary<gunType, GameObject> GunsData;

    void Start()
    {

        //Starting Guns
        currentGun = startingGun;
        GunsData = new Dictionary<gunType, GameObject>();
        GunsData.Add(gunType.AR, rifleObj);
        GunsData.Add(gunType.SG, shotgunObj);
        GunsData.Add(gunType.P, pistolObj);

        tryGetGunObjFromType(startingGun).GetComponent<Gun>().setVisible(true);

        DefaultGun = rifleObj;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) //Rifle
            selectGun(gunType.AR);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Shotgun
            selectGun(gunType.SG);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Pistol
            selectGun(gunType.P);


        Debug.DrawRay(camera.transform.position, camera.transform.forward * 1000, Color.red); // traiettoria proiettile visibile
        if (Input.GetButton("Fire1") && isFireEnabled)//tasto sinistro mouse
        {
            tryGetGunObjFromType(currentGun).GetComponent<Gun>().fire(camera);
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
        if (currentGun != gun)
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
    
    
}





