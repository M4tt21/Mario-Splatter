using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatsScript : MonoBehaviour
{

    [Header("All Cheats")]
    public bool immunity = false;
    public bool infiniteStamina = false;
    public bool infiniteAmmo = false;
    public bool instaKill = false;
    

    public static CheatsScript instance = null;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void give99Lives()
    {
        SaveStateScript.instance.mario.GetComponent<PlayerController>().lives += 99;
    }

    public void skipLevel()
    {
        SaveStateScript.instance.nextLevel();
    }

    public void unlockAllWeapons()
    {
        SaveStateScript.instance.mario.GetComponent<GunsController>().unlockGun(GunsController.gunType.AR);
        SaveStateScript.instance.mario.GetComponent<GunsController>().unlockGun(GunsController.gunType.P);
        SaveStateScript.instance.mario.GetComponent<GunsController>().unlockGun(GunsController.gunType.SG);
    }
}
