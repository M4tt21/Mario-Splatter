using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using System;

public class MenuScript : MonoBehaviour
{
    public GameObject PopUp;
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
        PlayerPrefs.SetInt("CurrentLevel", 0);

        SceneManager.LoadScene(PlayerPrefs.GetInt("menu"));
    }
    public void Exit()
    {
        UnityEngine.Application.Quit();
    }

    public void updateSliders()
    {
        GameObject sliders = GameObject.FindGameObjectWithTag("SlidersSettings");
        Slider sliderSens = sliders.transform.Find("Sens").GetComponent<Slider>();
        Slider sliderVolume = sliders.transform.Find("Volume").GetComponent<Slider>();
        sliderSens.value = SettingsScript.instance.sens;
        sliderVolume.value = SettingsScript.instance.volume;
    }

    public void updateKeybinds()
    {
        GameObject keybinds = GameObject.FindGameObjectWithTag("KeybindsSettings");
        foreach(TextMeshProUGUI txt in keybinds.transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch(txt.gameObject.name)
            {
                case "JumpText":
                    txt.SetText("" + SettingsScript.instance.jumpKey);
                    break;

                case "ReloadText":
                    txt.SetText("" + SettingsScript.instance.reloadKey);
                    break;
                case "SprintText":
                    txt.SetText("" + SettingsScript.instance.sprintKey);
                    break;
                case "ARText":
                    txt.SetText("" + SettingsScript.instance.ARKey);
                    break;
                case "SGText":
                    txt.SetText("" + SettingsScript.instance.SGKey);
                    break;
                case "PText":
                    txt.SetText("" + SettingsScript.instance.PKey);
                    break;
            }
        }
    }

    public void setKeybind(string keybindName)
    {
        
        StartCoroutine(waitAndSetKeybind(keybindName));
        
    }

    IEnumerator waitAndSetKeybind(string keybindName)
    {
        KeyCode keyToSet = KeyCode.None;
        yield return new WaitForSecondsRealtime(0.2f);  
        while (!Input.anyKeyDown || Input.GetKey(KeyCode.Mouse0)) 
        {
            yield return null;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PopUp.SetActive(false);
            yield break;
        
        }
            


        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode) && kcode != KeyCode.Mouse0)
            {
                Debug.Log("KeyCode gi�: " + kcode);
                keyToSet = kcode;
            }
        }

        switch (keybindName)
        {
            case "Jump":
                SettingsScript.instance.jumpKey = keyToSet;
                break;
            case "Reload":
                SettingsScript.instance.reloadKey = keyToSet;
                break;
            case "Sprint":
                SettingsScript.instance.sprintKey = keyToSet;
                break;
            case "AR":
                SettingsScript.instance.ARKey = keyToSet;
                break;
            case "SG":
                SettingsScript.instance.SGKey = keyToSet;
                break;
            case "P":
                SettingsScript.instance.PKey = keyToSet;
                break;
        }




        updateKeybinds();
        PopUp.SetActive(false);
    }


    public void setSlidersSettings()
    {
        GameObject sliders = GameObject.FindGameObjectWithTag("SlidersSettings");
        SettingsScript.instance.sens = sliders.transform.Find("Sens").GetComponent<Slider>().value;
        SettingsScript.instance.volume = sliders.transform.Find("Volume").GetComponent<Slider>().value;
    }

    public void saveAllSettings()
    {
        SettingsScript.instance.submitChanges();

    }
     
}
