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
    public int ARammo;
    public int currentARammo;
    public int pistolammo;
    public int currentPistolammo;
    public int SGammo;
    public int currentSGammo;


    public PlayerDataset(PlayerController player)
    {
        currentHealth = player.marioHealth.currentHealth;
        currentShield = player.marioHealth.currentShield;
        currentStamina = player.marioHealth.currentStamina;
        lives = player.lives;
        starCount = player.starCount;
        score = player.score;
        ARammo = player.guns.
        
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

