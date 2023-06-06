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
        if (health <= 0 && !isDead)
        {
            death();
            CanvasScript.scoreValue += 1;
        }
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
            navMeshAgent.SetDestination(player.transform.position);
        updateAnimator();
    }
}
