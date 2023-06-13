using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SpikeController : EnemyController
{

    [Header("Spike Stats")]
    [SerializeField] public GameObject throwable;
    [SerializeField] public float forceMul = 5f;
    [SerializeField] public float cooldownTime = 2.5f;
    [SerializeField] public bool isCooldown = false;
    [SerializeField] public float spawnTime = 2.3f;
    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && !isDead)
        {
            death();
            player.GetComponent<PlayerController>().score += enemyScore;
        }
        else if(!isCooldown){
            if(throwable!= null)
            {

                StartCoroutine(spawnTimer());
                //Start throwing animation
                

            }
        }

    }

    private IEnumerator spawnTimer()
    {
        isCooldown = true;
        animator.SetTrigger("Spawn");
        
        yield return new WaitForSeconds(spawnTime/2);
        GameObject currentThrowable = Instantiate(throwable);
        currentThrowable.GetComponent<Rigidbody>().useGravity = false;
        currentThrowable.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
        yield return new WaitForSeconds(spawnTime / 2);

        animator.SetTrigger("Throw");
        currentThrowable.GetComponent<Rigidbody>().useGravity = true;
        currentThrowable.GetComponent<Rigidbody>().AddForce(transform.forward * forceMul, ForceMode.Impulse);
        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }

}
