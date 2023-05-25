using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject player;
    //public GameObject guns;
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("CurrentLevel", 3);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void LoadGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
    }
    public void LoadLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", 2);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        //guns.gameObject.SetActive(true);
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
}
