using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsScript : MonoBehaviour
{
    [Header("Settings")]
    public float sens;
    public float volume;
    public string reloadKey = "r";
    public string Jump = "space";
    public string Fire3 = "left shift";
    public string Fire1 = "mouse 0";
    public string Ar = "1";
    public string Pump = "2";
    public string Pistol = "3";
    
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
