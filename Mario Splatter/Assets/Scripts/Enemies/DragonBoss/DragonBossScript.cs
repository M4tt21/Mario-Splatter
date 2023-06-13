using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBossScript : EnemyController
{
    public enum bossActions { IDLE, SPIN, FIREBALL}
    [Header("General Boss Stats")]
    public bossActions bossStatus = bossActions.IDLE;
    public GameObject FireBall;
    
    public float turnSpeed = 50f;
    public bool isAttacking = false;
    
    private Vector3 fireBallSpawnPosition;
    private GameObject lavaPillar;
    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        lavaPillar = transform.Find("LavaPillar").gameObject;
        fireBallSpawnPosition = transform.Find("FireBallSpawn").position;
    }
    private void FixedUpdate()
    {
        Debug.Log(animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart"));
        Debug.Log("Is idle : " + animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
        if (!isAttacking)
        {
            switch (bossStatus)
            {
                case bossActions.IDLE://When idle turn towards player
                    turnTowardsPlayer(turnSpeed);
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
        yield return null;//Wait a bit for the animarot to Sync Up
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackStart")) yield return null; //Wait for the starting animation to end
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
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpinAttackEnd")) yield return null;
        //Reset the status
        bossStatus = bossActions.IDLE;
        isAttacking = false;
    }

    void spitFireBall()
    {
        GameObject currentFireball = Instantiate(FireBall);
        currentFireball.transform.position = fireBallSpawnPosition;
        currentFireball.GetComponent<Rigidbody>().AddForce(transform.forward * 10, ForceMode.Impulse);
    }


    IEnumerator fireBallAttack(int nBalls, float spinDgrsBtwBalls, float speed)
    {
        isAttacking = true;
        for (int i = 0; i < nBalls; i++)
        {
            animator.SetTrigger("spitFireBallStart");//Play the starting animation
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) yield return null;
            yield return null;//Wait a bit for the animator to Sync Up
            while (animator.GetCurrentAnimatorStateInfo(0).IsName("SpitFireBallStart")) yield return null; //Wait for the starting animation to end
            spitFireBall();
            while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) yield return null;

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
    //Coroutine Function that rolls numbers to decide if to attack and what attack to do, it only modifies the bossStatus directly
}
