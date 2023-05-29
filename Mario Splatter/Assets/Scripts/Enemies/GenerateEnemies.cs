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
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            GameObject currentEnemy = Instantiate(theEnemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            currentEnemy.SetActive(true);

            if(navMeshVerDistance<=0)
                StartCoroutine(moveDown(currentEnemy, new Vector3(transform.position.x, transform.position.y + navMeshVerDistance, transform.position.z)));
            else
                StartCoroutine(moveUp(currentEnemy, new Vector3(transform.position.x, transform.position.y + navMeshVerDistance, transform.position.z)));

            yield return new WaitForSecondsRealtime(3);
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
