using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SettingsScript : MonoBehaviour
{

    [Header("Settings")]
    public KeyCode escKey = KeyCode.Escape;
    public KeyCode reloadKey = KeyCode.R;
    public KeyCode jumpKey = KeyCode.G;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode shootKey = KeyCode.Mouse0;
    public KeyCode ARKey = KeyCode.Alpha1;
    public KeyCode SGKey = KeyCode.Alpha2;
    public KeyCode PKey = KeyCode.Alpha3;
    [Range(0f, 1f)]
    public float sens = 0.5f;
    [Range(0f, 1f)]
    public float volume = 0.5f;


    public static SettingsScript instance;

    void Start()
    {
        setAllDefault();
        submitChanges();
        instance = this;
    }

    void setAllDefault()
    {
        escKey = KeyCode.Escape;
        reloadKey = KeyCode.R;
        jumpKey = KeyCode.Space;
        sprintKey = KeyCode.LeftShift;
        shootKey = KeyCode.Mouse0;
        ARKey = KeyCode.Alpha1;
        SGKey = KeyCode.Alpha2;
        PKey = KeyCode.Alpha3;
        sens = 0.5f;
        volume = 0.5f;
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

    public void submitChanges()
    {
        //Sets the audio listener and the axis settings
        AudioListener.volume = volume;
    }
    
}
