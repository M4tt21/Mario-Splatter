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
        SaveStateScript.instance.loadSettings(); //load settings when game first opens
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

    public void submitChanges()
    {
        //Sets the audio listener and the axis settings
        AudioListener.volume = volume;
    }
    
}

[System.Serializable]
public class SettingsData
{
    public KeyCode escKey;
    public KeyCode reloadKey;
    public KeyCode jumpKey;
    public KeyCode sprintKey;
    public KeyCode shootKey;
    public KeyCode ARKey;
    public KeyCode SGKey;
    public KeyCode PKey;
    public float sens;
    public float volume;

    public SettingsData(SettingsScript settings) 
    { 
        escKey= settings.escKey;
        reloadKey= settings.reloadKey;
        jumpKey= settings.jumpKey;
        sprintKey= settings.sprintKey;
        shootKey= settings.shootKey;
        ARKey = settings.ARKey;
        SGKey = settings.SGKey;
        PKey = settings.PKey;
        sens = settings.sens;
        volume = settings.volume;
    }

    public void loadToGame(SettingsScript settings)
    {
        settings.escKey = escKey;
        settings.reloadKey = reloadKey;
        settings.jumpKey = jumpKey;
        settings.sprintKey = sprintKey;
        settings.shootKey = shootKey;
        settings.ARKey = ARKey;
        settings.SGKey = SGKey;
        settings.PKey = PKey;
        settings.sens = sens;
        settings.volume = volume;
        settings.submitChanges();
    }
}
