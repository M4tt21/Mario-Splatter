using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARScript : Gun
{

    void Start()
    {
        currentAmmo = magazineSize;
        isOutOfAmmo = false;
        ps = transform.Find("muzzle").Find("Particle System").GetComponent<ParticleSystem>();
    }


    public override void fire(GameObject camera)
    {
        if (isOnCooldown)
            return;

        if (currentAmmo == 0) { 
            isOutOfAmmo = true;
            return;
        }

        if (!CheatsScript.instance.infiniteAmmo)
            currentAmmo--;

        //Find what the player is shooting at
        RaycastHit hitPoint;
        //Find if and what is between the character and the enemy shot
        RaycastHit hitLine;


        AudioSource a = gameObject.GetComponent<AudioSource>();
        a.Play();
        ps.Play();

        bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);

        if (raycastResCamera && hitPoint.transform.CompareTag("Enemy"))
        {

            Vector3 origin = transform.Find("muzzle").transform.position;
            Vector3 target = hitPoint.point;

            Debug.DrawLine(origin, target, Color.green);

            Vector3 direction = (target - origin).normalized;

            bool isValid = Physics.Raycast(origin, direction, out (hitLine));


            if ((isValid && hitLine.transform.CompareTag("Enemy")))//If the line detects an enemy in between then damage that enemy
            {
                hitLine.collider.gameObject.GetComponent<EnemyHit>().Hit(bulletDMG * (CheatsScript.instance.instaKill ? 1000 : 1));
                playFX(hitLine.point, direction);
                Debug.Log("Colpito il nemico" + hitLine.collider + "" + hitLine.collider.gameObject.GetComponent<EnemyHit>());

            }
        }
        StartCoroutine(gunCooldownTime());
    }
}
