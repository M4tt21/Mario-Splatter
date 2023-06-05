using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KoopaController : EnemyController
{

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health<=0 && !isDead)
        {
            death(); 
            CanvasScript.scoreValue += 1;
        }
        else if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
            navMeshAgent.SetDestination(player.transform.position);
    }

    
}
