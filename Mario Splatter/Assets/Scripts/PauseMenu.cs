using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public GameObject player;
    public GameObject pauseMenu;
    public static bool isPaused;
    void Start()
    {
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            Debug.Log("ciao");
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
    public void Resume()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentLevel", 3);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void Options()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void Exit()
    {
        Application.Quit();

    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        Cursor.visible = true;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
        Cursor.visible = false;

    }
}

