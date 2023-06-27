using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SGScript : Gun
{
    [Header("ShotGun Specific Stats")]
    [SerializeField] public int bulletsPerShot = 12;
    [SerializeField] public float spread = 5f;
    // Start is called before the first frame update
    void Start()
    {

        currentAmmo = magazineSize;
        isOutOfAmmo = false;
        ps = transform.Find("muzzle").Find("Particle System").GetComponent<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
    }

    public override void fire(GameObject camera)
    {
        if (isOnCooldown)
            return;

        if (!CheatsScript.instance.infiniteAmmo)
        {
            if (currentAmmo <= 0)
            {
                if (ammoHeld <= 0)
                    audioSource.PlayOneShot(noAmmoSound);
                isOutOfAmmo = true;
                return;
            }


            currentAmmo--;
        }

        //Find what the player is shooting at
        RaycastHit hitPoint;
        //Find if and what is between the character and the enemy shot
        RaycastHit hitLine;


        audioSource.PlayOneShot(shotSound);
        ps.Play();

        bool raycastResCamera = Physics.Raycast(camera.transform.position, camera.transform.forward, out (hitPoint), Mathf.Infinity);

        //traiettoria proiettile debug
        if (raycastResCamera)
        {
            for (int i = 0; i < bulletsPerShot; i++)
            {
                Vector3 origin = transform.Find("muzzle").transform.position;
                Vector3 target = hitPoint.point;

                Debug.DrawLine(origin, target, Color.green);

                Vector3 direction = (target - origin).normalized;
                Debug.DrawRay(origin, direction);
                Debug.DrawRay(origin, getRNGShotDirection(direction), Color.cyan, 2f);
                bool isValid = Physics.Raycast(origin, getRNGShotDirection(direction), out (hitLine), range);

                Debug.Log("Colpito il collider" + hitLine.collider + " alle coordinate " + hitLine.point);
                if ((isValid && hitLine.transform.CompareTag("Enemy")))//If the line detects an enemy in between then damage that enemy
                {
                    hitLine.collider.gameObject.GetComponent<EnemyHit>().Hit(bulletDMG * (CheatsScript.instance.instaKill ? 1000 : 1));
                    playFX(hitLine.point, direction);
                    Debug.Log("Colpito il nemico" + hitLine.collider + "" + hitLine.collider.gameObject.GetComponent<EnemyHit>());
                }
                else if (isValid)
                {
                    playGroundFX(hitLine.point, direction, hitLine.normal);
                }
            }
        }
        StartCoroutine(gunCooldownTime());
    }

    Vector3 AddNoiseOnAngle(float min, float max)
    {
        // Find random angle between min & max inclusive
        float xNoise = Random.Range(min, max);
        float yNoise = Random.Range(min, max);
        float zNoise = Random.Range(min, max);

        // Convert Angle to Vector3
        Vector3 noise = new Vector3(
          Mathf.Sin(2 * Mathf.PI * xNoise / 360),
          Mathf.Sin(2 * Mathf.PI * yNoise / 360),
          Mathf.Sin(2 * Mathf.PI * zNoise / 360)
        );
        return noise;
    }

    Vector3 getRNGShotDirection(Vector3 direction)
    {
        
        return (direction + AddNoiseOnAngle(-spread, spread));
    } 
}