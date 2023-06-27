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

    public enum marioSkin {DEFAULT, SHIELD, STAM}
    
    private static float wSpeed = 5f;
    private static float rSpeed = 10f;
    private float currentSpeed = wSpeed;
    private Coroutine currentJumpCoroutine = null;
    private Vector3 velocity;
    private float gravity = -9.81f;
    [Header("Player Data")]
    public CharacterController controller;
    public Animator animator;
    public Transform cameraTransform;
    public GunsController guns;
    public CanvasScript canvasScript;
    public MarioHealth marioHealth;
    public marioSkin currentSkin = marioSkin.DEFAULT;

    [Header("Player Stats")]
    public float jumpspeed = 3f;
    public float mouseSens = 100f;
    private float rotation = 0;
    public int lives;
    public float immunitySec = 3f;
    public int starCount = 0;
    public int score = 0;



    public bool infStamina = false;
    public bool isShielded = false;


    public bool isImmune;
    public Vector3 startingPos;
    private int startingLives=3;
    private Vector3 desiredCameraPos;
    private GameObject marioSkinDefault;
    private GameObject marioSkinShield;
    private GameObject marioSkinStamina;
    private AudioSource audioSource;
    private Transform headPivot;
    private Transform cameraPivot;

    [Header("Sounds")] //Default keybinds needed only for debugging
    public AudioClip jumpSound;
    public AudioClip noAmmoSound;
    public AudioClip damageSound;
    public AudioClip loseLifeSound;
    public AudioClip gameOverSound;
    public AudioClip winMusic;
    public AudioClip winMarioHappy1;
    public AudioClip winMarioHappy2;





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
        marioSkinStamina = transform.Find("MarioStamina").gameObject;
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

        //Skin Handler
        if(infStamina)
        {
            if (currentSkin != marioSkin.STAM)
            {
                Debug.Log("Current skin " + currentSkin + " into STAM");
                deactivateAllSkins();
                setActiveSkin(marioSkin.STAM, true);
                currentSkin = marioSkin.STAM;
            }
        }
        else if(marioHealth.currentShield>0)
        {
            if (currentSkin != marioSkin.SHIELD)
            {
                Debug.Log("perche sono qua SHIELD");
                deactivateAllSkins();
                setActiveSkin(marioSkin.SHIELD, true);
                currentSkin = marioSkin.SHIELD;
            }
        }
        else
        {
            if (currentSkin != marioSkin.DEFAULT)
            {
                Debug.Log("perche sono qua DEFAULT");
                deactivateAllSkins();
                setActiveSkin(marioSkin.DEFAULT, true);
                currentSkin = marioSkin.DEFAULT;
            }
        }
        
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
        if (guns.isReloading)
        {
            StopCoroutine("restoreStamCoroutine");
            StartCoroutine("restoreStamCoroutine", restoreStamCoroutine());
            return;
        }

        if (guns.GetCurrentGun()==GunsController.gunType.P && Input.GetButtonDown("Fire1") && Time.timeScale!=0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !guns.isReEquipping)
            guns.fireCurrentGun();
        else if (guns.GetCurrentGun() != GunsController.gunType.P && Input.GetButton("Fire1") && Time.timeScale!=0 && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !guns.isReEquipping)
            guns.fireCurrentGun();

        if (!guns.isReEquipping) 
        { 
            if (Input.GetKeyDown(SettingsScript.instance.ARKey)) //Rifle
                guns.selectGun(GunsController.gunType.AR);
            else if (Input.GetKeyDown(SettingsScript.instance.SGKey)) //Shotgun
                guns.selectGun(GunsController.gunType.SG);
            else if (Input.GetKeyDown(SettingsScript.instance.PKey)) //Pistol
                guns.selectGun(GunsController.gunType.P);
        }
        //Can only reload if hes not running 
        if (guns.isCurrentGunEnabled && (guns.isCurrentGunOutOfAmmo || Input.GetKeyDown(SettingsScript.instance.reloadKey)))
        {
            guns.reloadCurrentGun();
        }


        //sprint
        if (marioHealth.currentStamina>0 && Input.GetKey(SettingsScript.instance.sprintKey) && (Input.GetAxis("Vertical") > 0) && !Input.GetButton("Horizontal"))
        {
            marioHealth.isStaminaConsuming = !CheatsScript.instance.infiniteStamina && !infStamina;
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
            StopCoroutine("restoreStamCoroutine");
            StartCoroutine("restoreStamCoroutine", restoreStamCoroutine());
            guns.enableCurrentGun();
        }

        if (Input.GetKeyDown(SettingsScript.instance.jumpKey) && controller.isGrounded && !animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Walking&Running Rifle -> Jump"))
        {
            guns.disableCurrentGun();
            velocity.y += Mathf.Sqrt(jumpspeed * -3.0f * gravity);
            animator.SetTrigger("Jump");
            audioSource.PlayOneShot(jumpSound);
            StopCoroutine("jumpCoroutine");
            StartCoroutine("jumpCoroutine", jumpCoroutine());
        }
    }
    IEnumerator jumpCoroutine()
    {
        Debug.Log("STARTO");
        yield return new WaitForSeconds(1.2f);
        guns.enableCurrentGun();
        Debug.Log("STOPPO");
    }


    IEnumerator restoreStamCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        marioHealth.isStaminaConsuming = false;
    }

    public void setActiveInfiniteStamina(bool value)
    {
        infStamina = value;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("DeathZone"))
        {
            death();
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
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (CheatsScript.instance.immunity)
        {
            return;
        }

        //Danneggiamenti del player
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.TryGetComponent(out EnemyHit enemyHit);
            takeDamage(enemyHit.controller.damageToPlayer);
        }

        if (other.gameObject.CompareTag("Trap"))
        {
            other.TryGetComponent(out TrapScript trapScript);
            takeDamage(trapScript.damageToPlayer);
        }
    }

    public void takeDamage(float value)
    {
        if (isImmune)
            return;

        isImmune = true;
        setActiveInfiniteStamina(false);
        if (marioHealth.TakeDamage(value) <= 0)
        {
            death();
            return;
        }
        audioSource.PlayOneShot(damageSound);
        giveImmunity(immunitySec);
    }



    public void setActiveSkin(marioSkin skin, bool value)
    {
        switch(skin)
        {
            case marioSkin.DEFAULT:
                marioSkinDefault.SetActive(value);
                break;
            case marioSkin.SHIELD:
                marioSkinShield.SetActive(value);
                break;
            case marioSkin.STAM:
                marioSkinStamina.SetActive(value);
                break;
        }
    }

    public void deactivateAllSkins()
    {
        setActiveSkin(marioSkin.DEFAULT, false);
        setActiveSkin(marioSkin.SHIELD, false);
        setActiveSkin(marioSkin.STAM, false);
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

        //Immunity Effect
        float blinkDuration = 0.1f;
        int timesToBlink = (int)Math.Round((time / blinkDuration), 0)/2;
        for (int i = 0; i < timesToBlink; i++)
        {
            Renderer[] currentRenderers = getActiveRenderers();
            setActiveRenderers(currentRenderers, false);
            yield return new WaitForSeconds(blinkDuration);
            setActiveRenderers(currentRenderers, true);
            yield return new WaitForSeconds(blinkDuration);
        }
        isImmune = false;
    }

    private Renderer[] getActiveRenderers()
    {
        List<Renderer> activeRenderers = new List<Renderer>();
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null && renderer.enabled)
            {
                activeRenderers.Add(renderer);
            }
        }
        return activeRenderers.ToArray();
    }

    private void setActiveRenderers(bool value)
    {
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null)
            {
                renderer.enabled = value;
            }
        }
    }

    private void setActiveRenderers(Renderer[] renderers, bool value)
    {
        foreach (Renderer renderer in renderers)
        {
            if (renderer != null)
            {
                renderer.enabled = value;
            }
        }
    }

    private void giveImmunity(float time)
    {
        StartCoroutine(immunityTime(time));
    }

    public void death()
    {
        StartCoroutine(deathEvent());
    }

    private IEnumerator deathEvent()
    {
        if (SaveStateScript.instance.isLoading)
            yield break;
        isImmune = true;
        SaveStateScript.instance.isLoading = true;
        Time.timeScale = 0.1f;
        audioSource.PlayOneShot(loseLifeSound);
        yield return new WaitForSecondsRealtime(loseLifeSound.length);



        Time.timeScale = 1f;
        SaveStateScript.instance.isLoading = false;

        lives--;
        if (lives <= 0)
        {
            gameOver();
            yield break;
        }

        setActiveInfiniteStamina(false);
        giveImmunity(immunitySec);
        marioHealth.fullHealth();

        canvasScript.showLoseLifeScreen();
        transform.position = startingPos;
    }

    public IEnumerator starEvent()
    {
        SaveStateScript.instance.isLoading = true;
        Time.timeScale = 0.1f;
        audioSource.PlayOneShot(winMusic);

        yield return new WaitForSecondsRealtime(winMusic.length);

        switch (UnityEngine.Random.Range(1, 2))
        {
            case 1:
                audioSource.PlayOneShot(winMarioHappy1);
                break;
            case 2:
                audioSource.PlayOneShot(winMarioHappy2);
                break;
        }
        yield return new WaitForSecondsRealtime(3f);
        Time.timeScale = 1f;
        SaveStateScript.instance.isLoading = false;
        SaveStateScript.instance.nextLevel();
    }
    public IEnumerator gameOverEvent()
    {
        canvasScript.showGameOverScreen();
        SaveStateScript.instance.isLoading = true;
        Time.timeScale = 0.1f;
        audioSource.PlayOneShot(gameOverSound);
        yield return new WaitForSecondsRealtime(gameOverSound.length);
        Time.timeScale = 1f;
        SaveStateScript.instance.isLoading = false;
        SaveStateScript.instance.loadLevel(0);
    }

    public IEnumerator youWinEvent()
    {
        canvasScript.showYouWinScreen();
        Time.timeScale = 0.05f;
        yield return new WaitForSecondsRealtime(5);
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

    public void youWin()
    {
        StartCoroutine(youWinEvent());
    }

}