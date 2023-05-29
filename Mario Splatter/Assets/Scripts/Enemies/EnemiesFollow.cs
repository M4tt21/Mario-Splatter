using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesFollow : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
            
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.isOnNavMesh)
            navMeshAgent.SetDestination(player.position);
    }
}
