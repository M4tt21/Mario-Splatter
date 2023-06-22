using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArenaControllerScript : MonoBehaviour
{

    [SerializeField]
    GameObject bossToInstantiate;
    [SerializeField]
    Transform bossSpawnPosition;
    [SerializeField]
    GameObject arenaDoor;
    [SerializeField]
    GameObject winDescend;
    [SerializeField]
    GameObject[] winToDeactivate;
    [SerializeField]
    bool isDescended = false;
    [SerializeField]
    GameObject bossInstantiated = null;
    // Start is called before the first frame update
    [Header("Pickups Info")]
    public GameObject[] spawnablePickups;
    
    public Transform[] spawnPositions;
    public float pickUpSpawnCD = 10f;

    
    private bool isInside = false;
    void Start()
    {
        winDescend.GetComponent<Rigidbody>().useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bossInstantiated != null && bossInstantiated.GetComponent<EnemyController>().isDead && !isDescended) //Check when the boss dies
        {
            winDescend.GetComponent<Rigidbody>().useGravity = true;
            foreach(GameObject obj in winToDeactivate)
            {
                obj.SetActive(false);
            }

            isDescended = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bossInstantiated==null)
        {   
            //CLOSE THE ARENA DOOR
            arenaDoor.SetActive(true);

            isInside = true;
            StartCoroutine(pickUpsCoroutine());

            bossInstantiated = Instantiate(bossToInstantiate);
            bossInstantiated.transform.position = bossSpawnPosition.position;
            bossInstantiated.transform.rotation = bossSpawnPosition.rotation;
        }
    }

    public void despawnBoss(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bossInstantiated!=null)
        {
            if(!bossInstantiated.GetComponent<EnemyController>().isDead) 
            { 
                Destroy(bossInstantiated);
                arenaDoor.SetActive(false);
                isInside = false;
                bossInstantiated =null;
            }
        }
    }

    IEnumerator pickUpsCoroutine()
    {
        GameObject[] spawnedPickups = new GameObject[spawnPositions.Length];
        while (isInside)
        {
            yield return new WaitForSeconds(pickUpSpawnCD);

            if(spawnedPickups != null)
                foreach (GameObject pickup in spawnedPickups)
                {
                    if (pickup != null)
                        Destroy(pickup);
                }

            for (int i=0; i<spawnPositions.Length; i++) 
            {
                spawnedPickups[i] = Instantiate(spawnablePickups[Random.Range(0, spawnablePickups.Length)]);
                spawnedPickups[i].transform.position = new Vector3(spawnPositions[i].transform.position.x, spawnPositions[i].transform.position.y + 1, spawnPositions[i].transform.position.z);
            }
        }

        foreach(GameObject pickUp in spawnedPickups)
        {
            if (pickUp != null)
                Destroy(pickUp);
        }
    }
}
