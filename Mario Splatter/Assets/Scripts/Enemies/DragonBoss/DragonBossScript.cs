using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DragonBossScript : EnemyController
{
    public enum bossActions { SPAWN, IDLE, SPIN, FIREBALL}
    
    
    

    [Header("Boss Data")]
    public Transform fireBallSpawnPosition;
    public GameObject lavaPillar;
    public GameObject FireBall;

    [Header("Boss Stats")]
    public float turnSpeed = 50f;
    public bool isAttacking = false;
    public bossActions bossStatus = bossActions.SPAWN;

    [Header("Boss Attacks Chances Rolled on Fixed Update")]
    public float fireballChance = .001f;
    public float spinChance = .004f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }

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
    }
    private void FixedUpdate()
    {
        if (health <= 0 && !isDead)
        {
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
                    rollForAttack();
                    break;
                case bossActions.SPIN:
                    StartCoroutine(spinAttack(turnSpeed/5));
                    break;
                case bossActions.FIREBALL:
                    StartCoroutine(fireBallAttack(10, 36, turnSpeed));
                    break;
            }
        }
    }

    IEnumerator waitForSpawn()
    {
        while(animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn") || animator.GetCurrentAnimatorStateInfo(0).IsName("Spawn -> Idle")) yield return null;
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
        currentFireball.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
    }


    IEnumerator fireBallAttack(int nBalls, float spinDgrsBtwBalls, float speed)
    {
        isAttacking = true;
        for (int i = 0; i < nBalls; i++)
        {
            animator.SetTrigger("spitFireBallStart");//Play the starting animation
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || //Wait a bit for the animator to Sync Up
                animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("Idle -> SpitFireBallStart") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart -> SpitFireBall")) yield return null; //Wait for the starting animation to end
            spitFireBall();
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBall") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBall -> SpitFireBallEnd") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallEnd") ||
                animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallEnd -> Idle")) yield return null;

            //Spin Around
            Quaternion desiredRot = transform.rotation * Quaternion.Euler(Vector3.up * spinDgrsBtwBalls);
            while (transform.rotation != desiredRot)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, speed * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
        //Reset the status
        bossStatus = bossActions.IDLE;
        isAttacking = false;
    }
    //Function that rolls numbers to decide if to attack and what attack to do, it only modifies the bossStatus directly
    public void rollForAttack()
    {
        float randomN = Random.Range(0f, 1f);
        Debug.Log("Rolled a <" + randomN + "> for the Boss.");
        if(randomN < fireballChance)
        {
            Debug.Log(randomN + " < " + "" + fireballChance);
            bossStatus = bossActions.FIREBALL;
        }
        else if(randomN < fireballChance + spinChance)
        {
            Debug.Log(randomN + " < " + fireballChance + " + " + spinChance);
            bossStatus = bossActions.SPIN;
        }
        else
        {
            return;
        }
    }

    public override void death()
    {
        Debug.Log("BOSS SCONFITTO");
        animator.SetTrigger("death");
        isDead = true;
    }
}
