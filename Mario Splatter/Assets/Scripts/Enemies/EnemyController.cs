using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [Header("Enemy Stats")]

    [SerializeField] public float maxHealth = 300;
    [SerializeField]public float health = 300;
    [SerializeField]public float headHitMul = 2f;
    [SerializeField]public float bodyHitMul = 1.2f;
    [SerializeField]public float legHitMul = 1f;
    [SerializeField] public float armHitMul = 1f;
    [SerializeField] public float ragdollTime = 60f;
    [SerializeField] public float damageToPlayer = 10f;
    [SerializeField] public int enemyScore = 1;
    public GameObject ammoDrop;
    public bool isDead=false;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    public AudioSource audioSource;
    [Header("NavMeshData")]
    public GameObject player;

    

    // Start is called before the first frame update
    void Start()
    {
        initHealth(maxHealth);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = gameObject.GetComponent<AudioSource>();

    }
    

    public void initHealth(float health)
    {
        this.health = health;
    }

    public virtual void death()
    {
        //morte
        activateRagdoll();
        isDead = true;
        DropAmmo();
        enabled=false;
        //Destroy(gameObject);

    }
    public void DropAmmo()
    {
        GameObject drop = Instantiate(ammoDrop).gameObject;
        drop.transform.position = new Vector3(transform.position.x, transform.position.y + 1,transform.position.z); 
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

    public IEnumerator ragdollTimer()
    {
        yield return new WaitForSeconds(ragdollTime);
        Destroy(gameObject);
    }


    public void activateRagdoll()
    {
        foreach (Rigidbody rigidbody in transform.GetComponentsInChildren<Rigidbody>())
        {
            if (rigidbody != null)
            {
                rigidbody.velocity=Vector3.zero;
            }
        }

        foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
        {
            if(collider!=null)
            {
                collider.gameObject.layer = 2; //Set Layer to ignore raycast
                collider.enabled = true;
                collider.isTrigger = false;
                
            }
        }
        if (navMeshAgent != null) navMeshAgent.enabled = false;
        if (animator != null) animator.enabled = false;
        StartCoroutine(ragdollTimer());
        
    }

    public void updateAnimator()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
    }
}
