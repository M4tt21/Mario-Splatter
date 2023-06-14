using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public bool isPaused { private set; get; }


    [SerializeField]
    public TextMeshProUGUI score;
    [SerializeField]
    public TextMeshProUGUI ammoCounter;
    [SerializeField]
    public TextMeshProUGUI livesCounter;


    [SerializeField]
    public Transform AssaultRifleAim;
    [SerializeField]
    public Transform ShotgunAim;
    [SerializeField]
    public Transform PistolAim;

    private Dictionary<GunsController.gunType, Transform> GunsCrossair;
    private GunsController.gunType currentCrossair;

    [SerializeField]
    public GameObject UI;
    [SerializeField]
    public StatusBarsScript statusBarsScript;
    [SerializeField]
    public GameObject PauseMenu;
    [SerializeField]
    public GameObject OptionsMenuPausa;
    [SerializeField]
    public GameObject LoseLifeScreen;
    [SerializeField]
    public GunsController guns;
    [SerializeField]
    public PlayerController player;
    [SerializeField]
    public MarioHealth marioHealth;
    [SerializeField]
    public MenuScript menuScript;
    // Start is called before the first frame update
    void Start()
    {
        //Start Crossairs
        currentCrossair = GunsController.startingGun;
        GunsCrossair = new Dictionary<GunsController.gunType, Transform>
        {
            { GunsController.gunType.AR, AssaultRifleAim },
            { GunsController.gunType.SG, ShotgunAim },
            { GunsController.gunType.P, PistolAim }
        };
        player = SaveStateScript.instance.mario.GetComponent<PlayerController>();
        guns = player.guns;
        marioHealth = player.marioHealth;
    }


    void Update()
    {
        if (Input.GetKeyDown(SettingsScript.instance.escKey))
        {
            if (isPaused)
            {
                ResumeGame();
            }

            else
            {
                PauseGame();
            }
        }
    }

    void FixedUpdate()
    {
        if (player == null)
            return;
        ammoCounter.SetText(guns.getAmmoOfCurrentGun() + "|" + guns.getAmmoHeldOfCurrentGun());
        score.SetText("" + player.score);
        livesCounter.SetText("" + player.lives);
        updateCrossAir(guns.GetCurrentGun());
        statusBarsScript.updateStatus(marioHealth);
    }

    public void updateCrossAir(GunsController.gunType gun)
    {

        if (GunsCrossair != null && GunsCrossair.TryGetValue(gun, out Transform newCrossair) && GunsCrossair.TryGetValue(currentCrossair, out Transform oldCrossair))
        {
            oldCrossair.gameObject.SetActive(false);
            newCrossair.gameObject.SetActive(true);
            //Cambio Mirino

        }
        currentCrossair = gun;
    }

    

    private IEnumerator loseLifeScreenTime()
    {
        LoseLifeScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        LoseLifeScreen.SetActive(false);

    }

    public void showLoseLifeScreen()
    {
        Debug.Log("Active ? " + gameObject.activeInHierarchy);
        StartCoroutine(loseLifeScreenTime());
    }
    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        UI.SetActive(false);
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
        OptionsMenuPausa.SetActive(false);
        UI.SetActive(true);
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
}

