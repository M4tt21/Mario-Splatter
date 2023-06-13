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

    private SettingsScript settings;
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

    // Start is called before the first frame update
    void Start()
    {
        
        settings = gameObject.GetComponent<SettingsScript>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingPos = transform.position;
        lives = startingLives;
        desiredCameraPos = cameraTransform.localPosition;
        marioSkinDefault = transform.Find("MarioDefault").gameObject;
        marioSkinShield = transform.Find("MarioShield").gameObject;
        Input.ResetInputAxes();
        rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        float hzMove = Input.GetAxis("Horizontal") * currentSpeed * Time.deltaTime;
        float vtMove = Input.GetAxis("Vertical") * currentSpeed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

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

        Vector3 moveTo;
        //Collisione Telecamera
        Debug.DrawLine(transform.Find("Head").position, transform.Find("CameraPivot").position);
        if (Physics.Linecast(transform.Find("Head").position, transform.Find("CameraPivot").position, out RaycastHit cameraHit) && !cameraHit.transform.CompareTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.Find("CameraPivot").position, cameraHit.point);
            //moveTo = Mathf.Clamp((hit.distance * 0.87f), minDistance, maxDistance);
            cameraTransform.localPosition = Vector3.MoveTowards(desiredCameraPos, transform.Find("Head").localPosition, distance*1.15f);

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

        //Can only reload if hes not running !guns.isReEquipping && (guns.isCurrentGunOutOfAmmo || 
        if (guns.isCurrentGunEnabled && (guns.isCurrentGunOutOfAmmo || Input.GetKeyDown(settings.reloadKey)))
        {
            guns.reloadCurrentGun();
        }


        //sprint
        if (marioHealth.currentStamina>0 && Input.GetButton("Fire3") && (Input.GetAxis("Vertical") > 0) && !Input.GetButton("Horizontal"))
        {
            marioHealth.isStaminaConsuming = true;
            if (currentSpeed < rSpeed) currentSpeed += rSpeed * Time.deltaTime;
            //Se corre disabilito l'arma che ha in mano
            guns.disableCurrentGun();
        }
        else if (!(marioHealth.currentStamina > 0) || !Input.GetButton("Fire3") || Input.GetButton("Horizontal") || (Input.GetAxis("Vertical") < 0))
        {
            if (currentSpeed > wSpeed) currentSpeed -= wSpeed * Time.deltaTime;
        }

        if (Input.GetButtonUp("Fire3"))
        {
            marioHealth.isStaminaConsuming = false;
            guns.enableCurrentGun();
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y += Mathf.Sqrt(jumpspeed * -3.0f * gravity);
            animator.SetBool("isJumping", true);
        }
        else animator.SetBool("isJumping", false);

    }

    
    void OnCollisionEnter(Collision collision)
    {
    }
    void OnTriggerEnter(Collider collision)
    {
        
        if (collision.gameObject.CompareTag("NextLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));

        }

        if (collision.gameObject.CompareTag("PrevLevel"))
        {
            PlayerPrefs.SetInt("CurrentLevel", (SceneManager.GetActiveScene().buildIndex - 1) % SceneManager.sceneCountInBuildSettings);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));

        }

        if (collision.gameObject.CompareTag("Level1"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 2);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }

        if (collision.gameObject.CompareTag("Level2"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 3);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }

        if (collision.gameObject.CompareTag("Level3"))
        {
            PlayerPrefs.SetInt("CurrentLevel", 4);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        }

        if (collision.gameObject.CompareTag("DeathZone"))
        {
            death();
            giveImmunity(immunitySec);
        }

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
            Debug.Log("HAI PERSO");//CAMBIAREEEEEEE RESET LIVELLO

        marioHealth.fullHealth();

        canvasScript.showLoseLifeScreen();
        transform.position = startingPos;

    }

    public void nextLevelTeleport()
    {
        PlayerPrefs.SetInt("CurrentLevel", (SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }

    public IEnumerator starEvent()
    {
        Debug.Log("L'evento stella e` in esecuzione, teletrasporto al prossimo livello in qualche secondo.");

        yield return new WaitForSeconds(4);
        nextLevelTeleport();
    }
    
    public void starNextlevel()
    {
        starCount++;
        StartCoroutine(starEvent());
    }



}