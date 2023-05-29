using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject theEnemy;
    public int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Instantiate(theEnemy, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity).SetActive(true);
            yield return new WaitForSecondsRealtime(3);
            enemyCount += 1;
        }
    }
}
