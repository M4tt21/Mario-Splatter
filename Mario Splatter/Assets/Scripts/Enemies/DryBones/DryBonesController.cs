using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DryBonesController : EnemyController
{

    [Header("DryBones Stats")]

    [SerializeField] public float resTime = 5f;

    [Header("DryBones Sounds")]
    public AudioClip downSound;
    public AudioClip resSound;
    public AudioClip upSound;


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
        audioSource.PlayOneShot(downSound);
        navMeshAgent.isStopped=true;
        isDead = true;
        animator.SetTrigger("death");
        DropAmmo();
        StartCoroutine(resurrectTimer());
        
        
    }
    public IEnumerator resurrectTimer()
    {
        yield return new WaitForSeconds(resTime);//Wait for the resurrect timer then ressurect and communicate with animator
        isDead = false;
        animator.SetTrigger("resurrect");
        audioSource.PlayOneShot(resSound);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Death2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Death2 -> Resurrect")) yield return null;//Wait for the animation to finish
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Resurrect") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime<0.5) yield return null;
        audioSource.PlayOneShot(upSound);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Resurrect")) yield return null;

        activateColliderRaycast(true);
        navMeshAgent.isStopped = false;
    }
}
