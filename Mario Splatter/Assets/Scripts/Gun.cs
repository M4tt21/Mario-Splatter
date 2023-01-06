using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private int playerLayerMask;
    private GameObject gun;
    private GameObject camera;
    private GameObject rifle;
    private GameObject shotgun;
    private GameObject pistol;
    // Start is called before the first frame update
    void Start()
    {
        playerLayerMask = 2 << LayerMask.NameToLayer("Ignore Raycast");
        playerLayerMask = ~playerLayerMask;

        gun = GameObject.FindGameObjectWithTag("Guns");
        camera= GameObject.FindGameObjectWithTag("MainCamera");
        rifle=GameObject.FindGameObjectWithTag("rifle");
        shotgun=GameObject.FindGameObjectWithTag("shotgun");
        pistol=GameObject.FindGameObjectWithTag("pistol");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        
        Debug.DrawRay(camera.transform.position, camera.transform.forward * 1000, Color.red); // traiettoria proiettile visibile
        RaycastHit hitPoint;

        if (Input.GetButton("Fire1"))//tasto sinistro mouse
        {
            bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);
            bool isNotValid = Physics.Linecast(rifle.transform.position,hitPoint.transform.position,playerLayerMask);
            Debug.DrawLine(rifle.transform.GetChild(0).transform.position, hitPoint.transform.position) ;
            Debug.Log("Collision: " + isNotValid); //traiettoria proiettile debug
            if (raycastResCamera && isNotValid)
            {

                //Debug.Log("Collision at: " + hitPoint.distance); //traiettoria proiettile debug
               
               
                //Alternativa per distruggere i cloni
                if (hitPoint.transform.CompareTag("Blooper"))
                {
                    Destroy(hitPoint.transform.gameObject);
                }
            }
        }
    }
}
