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
    public Transform AssaultRifleAim;
    [SerializeField]
    public Transform ShotgunAim;
    [SerializeField]
    public Transform PistolAim;

    private Dictionary<Guns.gunType, Transform> GunsCrossair;
    private Guns.gunType currentCrossair;

    [SerializeField]
    public GameObject UI;
    [SerializeField]
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {

        //Start Crossairs
        currentCrossair = Guns.startingGun;
        GunsCrossair = new Dictionary<Guns.gunType, Transform>();
        GunsCrossair.Add(Guns.gunType.AR, AssaultRifleAim);
        GunsCrossair.Add(Guns.gunType.SG, ShotgunAim);
        GunsCrossair.Add(Guns.gunType.P, PistolAim);


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
       score.SetText("" + scoreValue);
    }

    public void updateCrossAir(Guns.gunType gun)
    {
        if (currentCrossair != gun)
        {
            if (GunsCrossair.TryGetValue(gun, out Transform newCrossair) && GunsCrossair.TryGetValue(currentCrossair, out Transform oldCrossair))
            {
                oldCrossair.gameObject.SetActive(false);
                newCrossair.gameObject.SetActive(true);

                //Cambio Mirino

            }
            currentCrossair = gun;
        }
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
}

