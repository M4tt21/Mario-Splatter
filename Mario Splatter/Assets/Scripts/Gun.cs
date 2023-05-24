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
    // Start is called before the first frame update
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
            bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);
            
            
             //traiettoria proiettile debug
            if (raycastResCamera && hitPoint.transform.CompareTag("Enemy"))
            {
                bool isNotValid = Physics.Linecast(rifle.transform.GetChild(0).transform.position,hitPoint.transform.position,out (hitLine),ignoreLayerMask);
                //Debug.Log("Collision at: " + hitPoint.distance); //traiettoria proiettile debug
                
                Debug.DrawLine(rifle.transform.GetChild(0).transform.position, hitPoint.transform.position) ;
                //Debug.Log("Qualcosa in mezzo? " + isNotValid);
                //Debug.DrawRay(new Vector3(0,0,0), hitLine.transform.position);
                //Alternativa per distruggere i cloni
                if (isNotValid && hitLine.transform.CompareTag("Enemy"))
                {
                    Destroy(hitPoint.transform.gameObject);
                    Debug.Log("Colpito il nemico");
                    CanvasScript.scoreValue += 1;
                }
            }
        }
    }
}
