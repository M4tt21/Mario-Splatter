using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;




public class PlayerController : MonoBehaviour
{
    private static float wSpeed = 5f;
    private static float rSpeed = 10f;
    private float currentSpeed = wSpeed;
    private Vector3 velocity;
    private float gravity = -9.81f;
    [Header("Player Data")]
    public CharacterController controller;
    public Animator animator;
    public Transform cameraTransform;
    public GunsController guns;
    public CanvasScript canvasScript;
    public MarioHealth marioHealth;

    [Header("Player Stats")]
    public float jumpspeed = 2;
    public float mouseSens = 100f;
    private float rotation = 0;
    public int lives;
    public float immunitySec = 3f;
    public int starCount = 0;
    public int score = 0;



    public bool isImmune;
    public Vector3 startingPos;
    private int startingLives=3;
    private Vector3 desiredCameraPos;
    private GameObject marioSkinDefault;
    private GameObject marioSkinShield;
    private AudioSource audioSource;
    private Transform headPivot;
    private Transform cameraPivot;

    [Header("Sounds")] //Default keybinds needed only for debugging
    public AudioClip jumpSound;



    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingPos = transform.position;
        lives = startingLives;
        desiredCameraPos = cameraTransform.localPosition;
        marioSkinDefault = transform.Find("MarioDefault").gameObject;
        marioSkinShield = transform.Find("MarioShield").gameObject;
        headPivot = transform.Find("Head");
        cameraPivot = transform.Find("CameraPivot");
        audioSource = transform.GetComponent<AudioSource>();
        Input.ResetInputAxes();
        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (SaveStateScript.instance.isLoading)
        { //If loading into scene disable all mario actions
            controller.Move(Vector3.zero);
            return;
        }

        float hzMove = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float vtMove = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * (mouseSens*SettingsScript.instance.sens) * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSens * SettingsScript.instance.sens) * Time.deltaTime;

        Vector3 move = transform.right * hzMove + transform.forward * vtMove;

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = gravity/3;
        }

        // Changes the height position of the player..

        

        velocity.y += gravity*2 * Time.deltaTime;
        controller.Move((velocity * Time.deltaTime) + move);

        


        float dotPF = Vector3.Dot(transform.forward, controller.velocity);
        float dotPH = Vector3.Dot(transform.right, controller.velocity);
        float dotPV = Vector3.Dot(transform.up, controller.velocity);

        animator.SetFloat("fVelocity", (float)Math.Round(dotPF, 2));
        animator.SetFloat("hVelocity", (float)Math.Round(dotPH,2));
        animator.SetFloat("vVelocity", (float)Math.Round(dotPV, 2));
        
        animator.SetFloat("turn", mouseX*10);

        if (dotPF == 0 && dotPH == 0 && dotPV == 0) animator.SetBool("isStill", true);

        //Change Mario Skin if he has shield
        activateSkinShield(marioHealth.currentShield > 0);

            //Movimento Telecamera
            rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, -80f, 80f);


        cameraTransform.localRotation = Quaternion.Euler(rotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        //Collisione Telecamera
        Debug.DrawLine(headPivot.position, cameraPivot.position);
        if (Physics.Linecast(headPivot.position, cameraPivot.position, out RaycastHit cameraHit) && !cameraHit.transform.CompareTag("Enemy"))
        {
            float distance = Vector3.Distance(cameraPivot.position, cameraHit.point);
            cameraTransform.localPosition = Vector3.MoveTowards(desiredCameraPos, headPivot.localPosition, distance*1.15f);

        }
        else
        {
            cameraTransform.localPosition = desiredCameraPos;
        }


        /*All Actions Below are unaccessible while reloading*/
        if (guns.isReloading && guns.isReEquipping)
        {
            marioHealth.isStaminaConsuming=false;
            return;
        }

        if(guns.GetCurrentGun()==GunsController.gunType.P && Input.GetButtonDown("Fire1") && Time.timeScale!=0)
            guns.fireCurrentGun();
        else if (guns.GetCurrentGun() != GunsController.gunType.P && Input.GetButton("Fire1") && Time.timeScale!=0)
            guns.fireCurrentGun();

        if (Input.GetKeyDown(KeyCode.Alpha1)) //Rifle
            guns.selectGun(GunsController.gunType.AR);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) //Shotgun
            guns.selectGun(GunsController.gunType.SG);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) //Pistol
            guns.selectGun(GunsController.gunType.P);

        //Can only reload if hes not running 
        if (guns.isCurrentGunEnabled && (guns.isCurrentGunOutOfAmmo || Input.GetKeyDown(SettingsScript.instance.reloadKey)))
        {
            guns.reloadCurrentGun();
        }


        //sprint
        if (marioHealth.currentStamina>0 && Input.GetKey(SettingsScript.instance.sprintKey) && (Input.GetAxis("Vertical") > 0) && !Input.GetButton("Horizontal"))
        {
            marioHealth.isStaminaConsuming = true && !CheatsScript.instance.infiniteStamina;
            if (currentSpeed < rSpeed) currentSpeed += rSpeed * Time.deltaTime;
            //Disable the gun when running
            guns.disableCurrentGun();
        }
        else if (!(marioHealth.currentStamina > 0) || !Input.GetKey(SettingsScript.instance.sprintKey) || Input.GetButton("Horizontal") || (Input.GetAxis("Vertical") < 0))
        {
            if (currentSpeed > wSpeed) currentSpeed -= wSpeed * Time.deltaTime;
        }

        if (Input.GetKeyUp(SettingsScript.instance.sprintKey))
        {
            marioHealth.isStaminaConsuming = false;
            guns.enableCurrentGun();
        }

        if (Input.GetKeyDown(SettingsScript.instance.jumpKey) && controller.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpspeed * -3.0f * gravity);
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(jumpSound);
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("NextLevel"))
        {
            SaveStateScript.instance.nextLevel();
        }

        if (collision.gameObject.CompareTag("PrevLevel"))
        {
            SaveStateScript.instance.prevLevel();
        }

        if (collision.gameObject.CompareTag("Level1"))
        {
            SaveStateScript.instance.loadLevel(2);
        }

        if (collision.gameObject.CompareTag("Level2"))
        {
            SaveStateScript.instance.loadLevel(3);
        }

        if (collision.gameObject.CompareTag("Level3"))
        {
            SaveStateScript.instance.loadLevel(4);
        }

        if (collision.gameObject.CompareTag("DeathZone"))
        {
            death();
            giveImmunity(immunitySec);
        }

        if (CheatsScript.instance.immunity)
        {
            return;
        }

        //Danneggiamenti del player
        if (collision.gameObject.CompareTag( "Enemy"))
        {
            
            if (isImmune)
                return;    

            if(collision.TryGetComponent(out EnemyHit enemyHit))
            {
                if (marioHealth.TakeDamage(enemyHit.controller.damageToPlayer)<=0)
                    death();
                giveImmunity(immunitySec);
            }
        }

        if (collision.gameObject.CompareTag("Trap"))
        {

            if (isImmune)
                return;

            if (collision.TryGetComponent(out TrapScript trapScript))
            {
                if (marioHealth.TakeDamage(trapScript.damageToPlayer) <= 0)
                    death();
                giveImmunity(immunitySec);
            }
        }

    }

    public void activateSkinShield(bool value)
    {
        marioSkinDefault.SetActive(!value);
        marioSkinShield.SetActive(value);
    }

    private int turnDirection(float Axis)
    {  //1=Right -1=Left
        if (Axis < 0) return -1;
        else if (Axis > 0) return 1;
        return 0;
    }

    private IEnumerator immunityTime(float time)
    {
        isImmune = true;
        yield return new WaitForSeconds(time);
        isImmune = false;
    }

    private void giveImmunity(float time)
    {
        StartCoroutine(immunityTime(time));
    }

    public void death()
    {
        lives--;
        if (lives <= 0) 
        { 
            gameOver();
            return;
        }

        marioHealth.fullHealth();

        canvasScript.showLoseLifeScreen();
        transform.position = startingPos;

    }

    public IEnumerator starEvent()
    {
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(4);
        Time.timeScale = 1f;
        SaveStateScript.instance.nextLevel();
    }
    public IEnumerator gameOverEvent()
    {
        canvasScript.showGameOverScreen();
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1f;
        SaveStateScript.instance.loadLevel(0);
    }

    public void starNextlevel()
    {
        starCount++;
        StartCoroutine(starEvent());
    }

    public void gameOver()
    {
        StartCoroutine(gameOverEvent());
    }


}