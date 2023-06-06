using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DryBonesController : EnemyController
{

    [Header("DryBones Stats")]

    [SerializeField] public float resTime = 5f;

    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && !isDead)
        {
            death();
            health = maxHealth;
            player.GetComponent<PlayerController>().score += enemyScore;
        }
        if (navMeshAgent.isOnNavMesh && !isDead)
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
    }

    public void activateColliderRaycast(bool value)
    {
        int layer;
        if (!value) { 
            layer = 2;
        }
        else
        {
            layer = 0;
        }
        foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
        {
            if (collider != null)
            {
                collider.gameObject.layer = layer; //Set Layer to ignore raycast
            }
        }
    }
    public override void death()
    {
        activateColliderRaycast(false);
        navMeshAgent.isStopped=true;
        isDead = true;
        animator.SetBool("isDead", isDead);
        StartCoroutine(resurrectTimer());
        
        
    }
    public IEnumerator resurrectTimer()
    {
        yield return new WaitForSeconds(resTime);//Wait for the resurrect timer then ressurect and communicate with animator
        isDead = false;
        animator.SetBool("isDead", isDead);
        yield return new WaitForSeconds(2f);//Wait for the animation to finish
        activateColliderRaycast(true);
        navMeshAgent.isStopped = false;
    }
}
