using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject player;
    //public GameObject guns;
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentLevel", 2);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void SelectLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", 1);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void Options()
    {
        
    }
    public void Exit()
    {
        Application.Quit();

    }
}
