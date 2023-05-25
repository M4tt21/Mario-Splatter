using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public static bool gunUpdate = true;
    public static int currentGun = 0;
    public static int scoreValue = 0;
    private bool isPaused;
    [SerializeField]
    public TextMeshProUGUI score;
    Transform[] crossAires;
    [SerializeField]
    public Transform AssaultRifleAim;
    [SerializeField]
    public Transform ShotgunAim;
    [SerializeField]
    public Transform PistolAim;

    [SerializeField]
    public GameObject UI;
    [SerializeField]
    public GameObject PauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        Transform[] ca= 
         {
            AssaultRifleAim,
            ShotgunAim,
            PistolAim
        };
        crossAires = ca;
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
       if (gunUpdate)
        {
            updateCrossAir(crossAires, currentGun);
            gunUpdate = false;
        }
    }
    void updateCrossAir(Transform[] crossAires, int gun)
    {
        int i = 0;
        foreach (Transform ca in crossAires)
        {
            if (i == gun)
                ca.gameObject.SetActive(true);
            else
                ca.gameObject.SetActive(false);
            i++;
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

