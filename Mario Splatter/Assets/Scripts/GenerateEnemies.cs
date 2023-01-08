using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject theEnemy;
    public GameObject pipe;
    public int enemyCount;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemyDrop());
    }
    IEnumerator EnemyDrop()
    {
        while (enemyCount < 30)
        {
            Instantiate(theEnemy, pipe.transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(3);
            enemyCount += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {


    }
}
