using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{

    [Header("Settings")]
    public KeyCode escKey = KeyCode.Escape;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode jumpKey = KeyCode.Space;
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
        //Set volume and sens
        sens = 0.5f;
        volume = 0.5f;
        submitChanges();
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

    public void submitChanges()
    {
        //Sets the audio listener and the axis settings
    }
    
}
