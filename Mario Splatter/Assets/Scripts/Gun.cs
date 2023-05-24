using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int ignoreLayerMask;
    private GameObject gun;
    private GameObject camera;
    private GameObject rifle;
    private GameObject shotgun;
    private GameObject pistol;
    KoopaHit kh;
    
    void Start()
    {
        ignoreLayerMask = 2 << LayerMask.NameToLayer("Ignore Raycast");
        ignoreLayerMask = ~ignoreLayerMask;

        gun = GameObject.FindGameObjectWithTag("Guns");
        camera= GameObject.FindGameObjectWithTag("MainCamera");
        rifle=GameObject.FindGameObjectWithTag("rifle");
        shotgun=GameObject.FindGameObjectWithTag("shotgun");
        pistol=GameObject.FindGameObjectWithTag("pistol");
    }

    private void FixedUpdate()
    {

        
        Debug.DrawRay(camera.transform.position, camera.transform.forward * 1000, Color.red); // traiettoria proiettile visibile
        RaycastHit hitPoint;
        RaycastHit hitLine;
        if (Input.GetButton("Fire1"))//tasto sinistro mouse
        {
            kh=null;
            bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);
            
            
             //traiettoria proiettile debug
            if (raycastResCamera && hitPoint.transform.CompareTag("EnemyHitBox"))
            {
                Debug.Log("Colpito il nemico");
                bool isNotValid = Physics.Linecast(rifle.transform.GetChild(0).transform.position,hitPoint.transform.position,out (hitLine),ignoreLayerMask);
                
                Debug.DrawLine(rifle.transform.GetChild(0).transform.position, hitPoint.transform.position) ;

                //Alternativa per distruggere i cloni
                if (isNotValid && hitLine.transform.CompareTag("EnemyHitBox"))
                {
                    //Destroy(hitPoint.transform.gameObject);
                    KoopaHit kh = hitPoint.transform.GetComponent<KoopaHit>();
                    kh.porcodio();

                    Debug.Log("Colpito il nemico"+hitPoint.transform.name);
                    //CanvasScript.scoreValue += 1;
                    /*switch (kh.ht)
                    {
                        case KoopaHit.hitType.head:
                            kh.Hit(20);
                            break;
                        case KoopaHit.hitType.body:
                            kh.Hit(10);
                            break;

                    }*/
                    
                }
            }
        }
    }
}
