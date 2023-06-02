using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public static int scoreValue = 0;
    private bool isPaused;


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
    public GameObject PauseMenu;
    [SerializeField]
    public GameObject LoseLifeScreen;
    [SerializeField]
    public GunsController guns;
    [SerializeField]
    public PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
       

        //se trova un canvas duplicato lo distrugge, sì può accadere
        GameObject[] oldCanvases = GameObject.FindGameObjectsWithTag("Canvas");
        foreach (GameObject oldCanvas in oldCanvases)
        {
            if (oldCanvases != null && oldCanvas != gameObject)
            {
                Debug.Log("Sono dentro");
                oldCanvas.transform.position = transform.position;
                Destroy(gameObject);
            }
        }
        Debug.Log("Non ho distrutto il canvas");
        GameObject.DontDestroyOnLoad(gameObject);// impedisce la distruzione immediata dell'oggetto

        //Start Crossairs
        currentCrossair = GunsController.startingGun;
        GunsCrossair = new Dictionary<GunsController.gunType, Transform>();
        GunsCrossair.Add(GunsController.gunType.AR, AssaultRifleAim);
        GunsCrossair.Add(GunsController.gunType.SG, ShotgunAim);
        GunsCrossair.Add(GunsController.gunType.P, PistolAim);


    }


    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
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
        ammoCounter.SetText(guns.getAmmoOfCurrentGun() + "|" + guns.getAmmoHeldOfCurrentGun());
        score.SetText("" + scoreValue);
        livesCounter.SetText("" + player.lives);
    }

    public void updateCrossAir(GunsController.gunType gun)
    {
        if (GunsCrossair.TryGetValue(gun, out Transform newCrossair) && GunsCrossair.TryGetValue(currentCrossair, out Transform oldCrossair))
        {
            oldCrossair.gameObject.SetActive(false);
            newCrossair.gameObject.SetActive(true);

            //Cambio Mirino

        }
        currentCrossair = gun;
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        UI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        UI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private IEnumerator loseLifeScreenTime()
    {
        LoseLifeScreen.SetActive(true);
        yield return new WaitForSeconds(2f);
        LoseLifeScreen.SetActive(false);

    }

    public void showLoseLifeScreen()
    {
        StartCoroutine(loseLifeScreenTime());
    }


}

