using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DragonBossScript : EnemyController
{
    public enum bossActions { SPAWN, IDLE, SPIN, FIREBALL}
    public Slider bossHealth;
    
    
    

    [Header("Boss Data")]
    public Transform fireBallSpawnPosition;
    public GameObject lavaPillar;
    public GameObject FireBall;

    [Header("Boss Stats")]
    public float turnSpeed = 50f;
    public bool isAttacking = false;
    public bossActions bossStatus = bossActions.SPAWN;
    public float minFireBallForceMul = 8f;
    public float maxFireBallForceMul = 20f;

    [Header("Boss Attacks Chances")]
    public float rollCD = 1f;
    [Range(0f, 1f)]
    public float fireballChance = 0.3f;
    [Range(0f, 1f)]
    public float spinChance = 0.1f;

    [Header("Boss Sounds")]
    public AudioClip spawnSound;
    public AudioClip fireballSpitSound;
    public AudioClip lavaPillarSound;


    private void Awake()
    {

        animator = gameObject.GetComponent<Animator>();
        bossStatus = bossActions.SPAWN;
        if(SaveStateScript.instance != null)
            player = SaveStateScript.instance.mario;
        else//Only for debugging purposes
            player = GameObject.FindGameObjectWithTag("Player");
        initHealth(maxHealth);
        StartCoroutine(waitForSpawn());
        StartCoroutine(rollForAttack());
    }
    private void FixedUpdate()
    {
        float fillBossHealthValue = health / maxHealth;
        bossHealth.value = fillBossHealthValue;
        Coroutine currentCoroutine = null;
        if (health <= 0 && !isDead)
        {
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
            death();
            return;
        }
        if (!isAttacking && !isDead)
        {
            
            
            switch (bossStatus)
            {
                case bossActions.SPAWN:
                    return;
                case bossActions.IDLE://When idle turn towards player
                    turnTowardsPlayer(turnSpeed);
                    break;
                case bossActions.SPIN:
                    StartCoroutine(spinAttack(turnSpeed/15));
                    break;
                case bossActions.FIREBALL:
                    StartCoroutine(fireBallAttack());
                    break;
            }
        }
    }

    IEnumerator waitForSpawn()
    {
        setCollidersHittable(false);
        audioSource.PlayOneShot(spawnSound);
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") || animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn -> Idle")) yield return null;
        setCollidersHittable(true);
        Debug.Log("Boss Finished Spawn animation");
        bossStatus = bossActions.IDLE;
    }


    void turnTowardsPlayer(float speed)
    {
        //Dont follow the player y position
        Quaternion desiredRot = Quaternion.LookRotation(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, speed * Time.deltaTime);
    }

    IEnumerator spinAttack(float speed)
    {
        isAttacking = true;
        animator.SetTrigger("spinAttackStart");//Play the starting animation
        audioSource.PlayOneShot(lavaPillarSound);
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || //Animarot needs to Sync Up
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle -> SpinAttackStart") || 
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackStart") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackStart -> SpinAttack")) yield return null; //Wait for the starting animation to end
        //Activate the Lava Pillar 
        lavaPillar.SetActive(true);
        //Spin Around
        float startRotation = transform.eulerAngles.y;
        float endRotation = startRotation + 360.0f;
        float t = 0.0f;
        while (t < speed)
        {
            t += Time.deltaTime;
            float yRotation = Mathf.Lerp(startRotation, endRotation, t / speed) % 360.0f;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, yRotation, transform.eulerAngles.z);
            yield return null;
        }
        //Deactivate the pillar
        lavaPillar.SetActive(false);
        //Play the Ending Animation
        animator.SetTrigger("spinAttackEnd");
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackEnd") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttack -> SpinAttackEnd") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackEnd -> Idle")) yield return null;
        //Reset the status
        bossStatus = bossActions.IDLE;
        isAttacking = false;
    }

    void spitFireBall()
    {
        GameObject currentFireball = Instantiate(FireBall);
        currentFireball.transform.position = fireBallSpawnPosition.position;
        currentFireball.transform.forward = fireBallSpawnPosition.forward;
        currentFireball.GetComponent<Rigidbody>().AddForce(fireBallSpawnPosition.forward * Random.Range(minFireBallForceMul, maxFireBallForceMul), ForceMode.Impulse);
    }


    IEnumerator fireBallAttack()
    {
        isAttacking = true;
        animator.SetTrigger("spitFireBallStart");//Play the starting animation
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || //Wait a bit for the animator to Sync Up
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("Idle -> SpitFireBallStart") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart -> SpitFireBall")) yield return null; //Wait for the starting animation to end
        audioSource.PlayOneShot(fireballSpitSound);
        spitFireBall();
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBall") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBall -> SpitFireBallEnd") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallEnd") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallEnd -> Idle")) yield return null;
        //Reset the status
        bossStatus = bossActions.IDLE;
        isAttacking = false;
    }
    //Function that rolls numbers to decide if to attack and what attack to do, it only modifies the bossStatus directly
    IEnumerator rollForAttack()
    {
        while (!isDead)
        {
            if (bossStatus == bossActions.IDLE)
            {
                float randomN = Random.Range(0f, 1f);
                Debug.Log("Rolled a <" + randomN + "> for the Boss.");
                if (randomN < fireballChance)
                {
                    Debug.Log(randomN + " < " + "" + fireballChance);
                    bossStatus = bossActions.FIREBALL;
                }
                else if (randomN < fireballChance + spinChance)
                {
                    Debug.Log(randomN + " < " + fireballChance + " + " + spinChance);
                    bossStatus = bossActions.SPIN;
                }
            }
            yield return new WaitForSeconds(rollCD);
        }
    }

    public override void death()
    {
        Debug.Log("BOSS SCONFITTO");
        StopAllCoroutines();
        lavaPillar.SetActive(false);
        animator.SetTrigger("death");
        isDead = true;
    }

    void setCollidersHittable(bool value)
    {
        foreach (Collider collider in transform.GetComponentsInChildren<Collider>())
        {
            if (collider != null)
            {
                collider.enabled = value;
            }
        }
    }
}
