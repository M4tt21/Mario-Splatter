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
    bool isDescended=false;
    [SerializeField]
    float descendAmmount = -10f;
    [SerializeField]
    GameObject bossInstantiated = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(bossInstantiated != null && bossInstantiated.GetComponent<EnemyController>().isDead && !isDescended) //Check when the boss dies
        {
            winDescend.transform.position = new Vector3 (winDescend.transform.position.x, winDescend.transform.position.y + descendAmmount, winDescend.transform.position.z);
            isDescended = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && bossInstantiated==null)
        {   
            //CLOSE THE ARENA DOOR
            arenaDoor.SetActive(true);
            
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
                bossInstantiated =null;
            }
        }
    }
}
