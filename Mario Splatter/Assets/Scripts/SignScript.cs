using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    void Update()
    {
        updateKeybindsSign();
    }
    public void updateKeybindsSign()
    {
        Debug.Log("Sono dentro");
        GameObject signs = GameObject.FindGameObjectWithTag("Signs");
        Debug.Log(signs);
        foreach (TextMeshProUGUI txt in signs.transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (txt.gameObject.name)
            {
                case "JumpSignText":
                    Debug.Log("Cartello salto");
                    txt.SetText("premi" +SettingsScript.instance.jumpKey + "per saltare");
                    Debug.Log("Cartello salto scritto");
                    break;

                case "SwitchWeaponSignText":
                    Debug.Log("Cartello cambio arma");
                    txt.SetText("premi" + SettingsScript.instance.PKey +  SettingsScript.instance.SGKey +  SettingsScript.instance.ARKey + "per scegliere l'arma da equipaggiare");
                    Debug.Log("Cartello cambio arma scritto");
                    break;
                case "SprintSignText":
                    Debug.Log("Cartello corsa");
                    txt.SetText("premi" + SettingsScript.instance.sprintKey + "per correre");
                    Debug.Log("Cartello corsa scritto");
                    break;

            }
        }
    }

}
