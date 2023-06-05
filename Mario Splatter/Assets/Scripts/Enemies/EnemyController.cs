using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]

    [SerializeField]public float health = 300;
    [SerializeField]public float headHitMul = 2f;
    [SerializeField]public float bodyHitMul = 1.2f;
    [SerializeField]public float legHitMul = 1f;
    [SerializeField] public float armHitMul = 1f;
    public bool isDead=false;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    [Header("NavMeshData")]
    public GameObject player;

    

    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (navMeshAgent.isOnNavMesh)
            navMeshAgent.SetDestination(player.transform.position);
        Debug.Log("2");
    }
    

    public void initHealth(float health)
    {
        this.health = health;
    }

    public void death()
    {
        //morte
        activateRagdoll();
        isDead = true;
        enabled=false;
        //Destroy(gameObject);
    }

    public void damage(float ammount, EnemyHit.hitType ht)
    {
        switch (ht){
            case EnemyHit.hitType.head:
                health -= ammount * headHitMul;
                break;
            case EnemyHit.hitType.body:
                health -= ammount * bodyHitMul;
                break;
            case EnemyHit.hitType.leg:
                health -= ammount * legHitMul;
                break;
            case EnemyHit.hitType.arm:
                health -= ammount * armHitMul;
                break;
        }
    }

    public void activateRagdoll()
    {
        navMeshAgent.enabled = false;
        Debug.Log("animator");
        animator.enabled = false;
        Debug.Log("animator OFFFFF?????");
        foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
        {
            if(collider!=null)
            {
                collider.enabled = true;
                Debug.Log(collider.gameObject + " disattivato trigger | Status trigger : " + collider.isTrigger);
                collider.isTrigger = false;
                Debug.Log(collider.gameObject + " disattivato trigger | Status trigger : " + collider.isTrigger);
            }
        }
    }
}
