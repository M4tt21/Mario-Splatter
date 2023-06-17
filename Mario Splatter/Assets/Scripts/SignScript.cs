using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SignScript : MonoBehaviour
{
    void Start()
    {
        updateKeybindsSign();
    }
    public void updateKeybindsSign()
    {
        GameObject signs = GameObject.FindGameObjectWithTag("Signs");
        foreach (TextMeshProUGUI txt in signs.transform.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (txt.gameObject.name)
            {
                case "JumpSign":
                    txt.SetText("premi" + SettingsScript.instance.jumpKey + "per saltare");
                    break;

                case "SwitchWeaponSign":
                    txt.SetText("premi" + SettingsScript.instance.PKey +  SettingsScript.instance.SGKey +  SettingsScript.instance.ARKey + "per scegliere l'arma da equipaggiare");
                    break;
                case "SprintSign":
                    txt.SetText("premi" + SettingsScript.instance.sprintKey + "per correre");
                    break;

            }
        }
    }

}
