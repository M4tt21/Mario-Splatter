using System.ComponentModel.Design;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


[System.Serializable]
public class PlayerDataset
{
    public SerializableVector3 startingPos;
    public float currentHealth;
    public float currentShield;  
    public float currentStamina;
    public int lives;
    public int starCount;
    public int score;
    public int level;
    public int ARAmmo;
    public int currentARAmmo;
    public int PAmmo;
    public int currentPAmmo;
    public int SGAmmo;
    public int currentSGAmmo;
    public bool isARUnlocked;
    public bool isSGUnlocked;
    public bool isPUnlocked;


    public PlayerDataset(PlayerController player)
    {
        currentHealth = player.marioHealth.currentHealth;
        currentShield = player.marioHealth.currentShield;
        currentStamina = player.marioHealth.currentStamina;
        lives = player.lives;
        starCount = player.starCount;
        score = player.score;
        ARAmmo = player.guns.getAmmoHeldOfGun(GunsController.gunType.AR);
        PAmmo = player.guns.getAmmoHeldOfGun(GunsController.gunType.P);
        SGAmmo = player.guns.getAmmoHeldOfGun(GunsController.gunType.SG);

        currentARAmmo = player.guns.getAmmoInGun(GunsController.gunType.AR);
        currentPAmmo = player.guns.getAmmoInGun(GunsController.gunType.P);
        currentSGAmmo = player.guns.getAmmoInGun(GunsController.gunType.SG);

        isARUnlocked = player.guns.isARUnlocked;
        isPUnlocked = player.guns.isPUnlocked;
        isSGUnlocked = player.guns.isSGUnlocked;

        level = PlayerPrefs.GetInt("CurrentLevel");
        
    

    }
    public void loadToPlayer(PlayerController player)
    {
        player.marioHealth.currentHealth = currentHealth;
        player.marioHealth.currentShield = currentShield;
        player.marioHealth.currentStamina = currentStamina;
        player.lives = lives;
        player.starCount = starCount;
        player.score = score;

        player.guns.setAmmoHeldOfGun(GunsController.gunType.AR, ARAmmo);
        player.guns.setAmmoHeldOfGun(GunsController.gunType.P, PAmmo);
        player.guns.setAmmoHeldOfGun(GunsController.gunType.SG, SGAmmo);

        player.guns.setAmmoInGun(GunsController.gunType.AR, currentARAmmo);
        player.guns.setAmmoInGun(GunsController.gunType.P, currentPAmmo);
        player.guns.setAmmoInGun(GunsController.gunType.SG, currentSGAmmo);

        player.guns.isARUnlocked = isARUnlocked;
        player.guns.isPUnlocked = isPUnlocked;
        player.guns.isSGUnlocked = isSGUnlocked;
    }
}


[System.Serializable]
public struct SerializableVector3
{
    float x, y, z;

    public SerializableVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
    }
    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }
}

