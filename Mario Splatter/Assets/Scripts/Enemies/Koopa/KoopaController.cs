using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoopaController : EnemyController
{
    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health<=0)
        {
            death(); 
            CanvasScript.scoreValue += 1;
        }
    }

    void Update()
    {
        if (navMeshAgent.isOnNavMesh)
            navMeshAgent.SetDestination(player.position);
    }
}
