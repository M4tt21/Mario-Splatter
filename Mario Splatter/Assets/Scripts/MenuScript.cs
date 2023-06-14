using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [SerializeField]
    public CanvasScript cs;
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
    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(PlayerPrefs.GetInt("menu"));
    }
    public void Exit()
    {
        Application.Quit();
    }

    public void SetSliders()
    {
        GameObject sliders = GameObject.FindGameObjectWithTag("SlidersSettings");
        Slider sliderSens = sliders.transform.Find("Sens").GetComponent<Slider>();
        Slider sliderVolume = sliders.transform.Find("Volume").GetComponent<Slider>();
        sliderSens.value = SettingsScript.instance.sens;
        sliderVolume.value = SettingsScript.instance.volume;
    }
}
