using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{

    [Header("Settings")]
    public KeyCode escKey = KeyCode.Escape;
    public string reloadKey = "r";
    public string Jump = "space";
    public string Fire3 = "left shift";
    public string Fire1 = "mouse 0";
    public string Ar = "1";
    public string Pump = "2";
    public string Pistol = "3";
    [Range(0f, 1f)]
    public float sens = 0.5f;
    [Range(0f, 1f)]
    public float volume = 0.5f;
    public static SettingsScript instance;

    void Start()
    {
        
        instance = this;
    }
    void updateButtonText()
    {

    }
    public void setJumpKey ()
    {   
        /*foreach (KeyCode kcode in Enum.GetValues ​​(typeof(KeyCode)) )
        {
            if (Input.GetKey(kcode))
                Debug.Log("KeyCode giù: " + kcode);
        }*/
        //Jump = Input.GetKey(out KeyCode keyCode);
    }
   void ChangeVolume ()
    {
        AudioListener.volume = volume;
    }
    void ChangeSens(PlayerController playerController)
    {
        playerController.mouseSens = sens;
    }
    
}
