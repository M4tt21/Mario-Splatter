using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    //public GameObject player;
    [SerializeField]
    public CanvasScript cs;

    void Start()
    {
        gameObject.SetActive(false);
    }
    void Update()
    {
       
    }
    public void Resume()
    {
        cs.ResumeGame();
    }
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(PlayerPrefs.GetInt("menu"));
        
    }
    public void Options()
    {
        
    }
    public void Exit()
    {
        Application.Quit();

    }
    
}

