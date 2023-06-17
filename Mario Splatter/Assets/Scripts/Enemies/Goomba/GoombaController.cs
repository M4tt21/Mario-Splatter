using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaController : EnemyController
{
    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && !isDead)
        {
            death();
            player.GetComponent<PlayerController>().score += enemyScore;
        }
        if (navMeshAgent.isOnNavMesh && navMeshAgent.isActiveAndEnabled)
            navMeshAgent.SetDestination(player.transform.position);
        updateAnimator();
    }
}
