using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateEnemies : MonoBehaviour
{
    [Header("Enemy Info")]
    public GameObject theEnemy;
    public int enemyCount;

    [Header("NavMesh drop-off Info")]
    public float navMeshVerDistance;
    public float speed;

    private Transform spawnLocation;
    private AudioSource audio;
    private bool isTriggered = false;
    // Start is called before the first frame update
    void Start()
    {
        spawnLocation = transform.Find("Generator");
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !isTriggered)
        {
            StartCoroutine(EnemyDrop());
            isTriggered = true;
            transform.GetComponent<Collider>().enabled = false;
        }
    }

    IEnumerator EnemyDrop()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject currentEnemy = Instantiate(theEnemy, new Vector3(spawnLocation.position.x, spawnLocation.position.y, spawnLocation.position.z), Quaternion.identity);
            currentEnemy.SetActive(true);

            if(navMeshVerDistance<=0)
                StartCoroutine(moveDown(currentEnemy, new Vector3(spawnLocation.position.x, spawnLocation.position.y + navMeshVerDistance, spawnLocation.position.z)));
            else
                StartCoroutine(moveUp(currentEnemy, new Vector3(spawnLocation.position.x, spawnLocation.position.y + navMeshVerDistance, spawnLocation.position.z)));
            if (audio!= null) audio.Play();
            yield return new WaitForSeconds(3);
        }
    }

    IEnumerator moveDown(GameObject currentEnemy, Vector3 moveTo)
    {
        currentEnemy.GetComponent<NavMeshAgent>().enabled = false;
        currentEnemy.GetComponent<Rigidbody>().useGravity = true;
        while (currentEnemy.transform.position.y >= moveTo.y) { yield return new WaitForEndOfFrame(); }
        currentEnemy.GetComponent<NavMeshAgent>().enabled = true;

    }

    IEnumerator moveUp(GameObject currentEnemy, Vector3 moveTo)
    {
        currentEnemy.GetComponent<NavMeshAgent>().enabled = false;
        currentEnemy.GetComponent<Rigidbody>().useGravity = true;
        while (currentEnemy.transform.position.y <= moveTo.y) {
            currentEnemy.transform.position = Vector3.Lerp(currentEnemy.transform.position, moveTo*2, Time.deltaTime * speed);
            yield return new WaitForEndOfFrame(); 
        }
        currentEnemy.GetComponent<NavMeshAgent>().enabled = true;

    }


}
