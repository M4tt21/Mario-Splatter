using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class koopaFollow : MonoBehaviour
{
    public NavMeshAgent Cube;
    public Transform player;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Cube.SetDestination(player.position);
    }
}
