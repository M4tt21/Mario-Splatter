using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaController : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        animator.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0)
        {
            death();
            CanvasScript.scoreValue += 1;
        }

    }

    void Update()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            animator.enabled = true;
            navMeshAgent.SetDestination(player.transform.position);
        }
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude/navMeshAgent.speed);
    }
}
