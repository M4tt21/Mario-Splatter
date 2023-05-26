using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guns : MonoBehaviour
{
    [SerializeField]
    private CanvasScript cs;
    public enum gunType { AR, SG, P }

    public static gunType startingGun = gunType.AR;
    private gunType currentGun;

    public gunType GetCurrentGun(){return currentGun;}
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
        ignoreLayerMask = 2 << LayerMask.NameToLayer("Ignore Raycast");
        ignoreLayerMask = ~ignoreLayerMask;

        //Starting Guns
        currentGun = startingGun;
        GunsData = new Dictionary<gunType, GameObject>();
        GunsData.Add(gunType.AR, rifleObj);
        GunsData.Add(gunType.SG, shotgunObj);
        GunsData.Add(gunType.P, pistolObj);

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
        if (Input.GetButton("Fire1"))//tasto sinistro mouse
        {
            fire();
        }
    }

    private void fire()
    {
        if (isOnCooldown)
            return;
        

        RaycastHit hitPoint;
        RaycastHit hitLine;
        
        AudioSource a = tryGetGunObjFromType(currentGun).GetComponent<AudioSource>();
        a.Play();
        kh = null;
        bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);


        //traiettoria proiettile debug
        if (raycastResCamera && hitPoint.transform.CompareTag("Enemy"))
        {
            Debug.Log("Colpito il nemico");
            bool isNotValid = Physics.Linecast(tryGetGunObjFromType(currentGun).transform.GetChild(0).transform.position, hitPoint.transform.position, out (hitLine), ignoreLayerMask);

            Debug.DrawLine(tryGetGunObjFromType(currentGun).transform.GetChild(0).transform.position, hitPoint.transform.position);

            //Alternativa per distruggere i cloni
            if (isNotValid && hitLine.transform.CompareTag("Enemy"))
            {
                //Destroy(hitPoint.transform.gameObject);
                KoopaHit kh = hitPoint.collider.gameObject.GetComponent<KoopaHit>();
                //kh.porcodio();

                //Debug.Log("Colpito il nemico"+hitPoint.collider +""+ hitPoint.collider.gameObject.GetComponent<KoopaHit>());
                switch (kh.ht)
                {
                    case KoopaHit.hitType.head:
                        Debug.Log("Colpita TESTA");
                        kh.Hit(20);
                        break;
                    case KoopaHit.hitType.body:
                        Debug.Log("Colpita CORPO");
                        kh.Hit(10);
                        break;

                }
            }
        }
        

        StartCoroutine(gunCooldownTime());
    }
    private IEnumerator gunCooldownTime()
    {
        isOnCooldown = true;
        switch (currentGun)
        {
            case gunType.AR:
                yield return new WaitForSeconds(ARCooldown);
                break;
            case gunType.SG:
                yield return new WaitForSeconds(SGCooldown);
                break;
            case gunType.P:
                yield return new WaitForSeconds(PCooldown);
                break;
        }
        isOnCooldown = false;
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
                oldGunObj.SetActive(false);
                newGunObj.SetActive(true);

                //Cambio Mirino
                cs.updateCrossAir(gun);
            }
            currentGun = gun;
        }
    }
}



