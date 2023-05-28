using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARScript : Gun
{
    // Start is called before the first frame update
    void Start()
    {
        //set stats for AR
        reloadTime = 1f;
        shotCD = 0.1f;
        magazineSize = 24;
        currentAmmo = magazineSize;
        isOutOfAmmo = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool fire(GameObject camera)
    {
        if (isOnCooldown)
            return false;

        if (currentAmmo == 0) { 
            isOutOfAmmo = true;
            return false;
        }

        currentAmmo--;

        //Find what the player is shooting at
        RaycastHit hitPoint;
        //Find if and what is between the character and the enemy shot
        RaycastHit hitLine;
        //stot successful
        bool result = false;

        AudioSource a = gameObject.GetComponent<AudioSource>();
        a.Play();

        EnemyHit enemyPartHit = null;
        bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);


        //traiettoria proiettile debug
        if (raycastResCamera && hitPoint.transform.CompareTag("Enemy"))
        {
            Debug.Log("Colpito il nemico");

            bool isNotValid = Physics.Linecast(transform.GetChild(0).transform.position, hitPoint.transform.position, out (hitLine));
            Debug.DrawLine(transform.GetChild(0).transform.position, hitPoint.transform.position);

            if (isNotValid && hitLine.transform.CompareTag("Enemy"))
            {
                enemyPartHit = hitPoint.collider.gameObject.GetComponent<EnemyHit>();

                enemyPartHit.Hit();

                result = true;

                //Debug.Log("Colpito il nemico"+hitPoint.collider +""+ hitPoint.collider.gameObject.GetComponent<KoopaHit>());

            }
        }
        StartCoroutine(gunCooldownTime());
        return result;
    }
}
