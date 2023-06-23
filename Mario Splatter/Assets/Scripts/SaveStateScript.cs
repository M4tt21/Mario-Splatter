using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SaveStateScript : MonoBehaviour
{
    //salvataggio
    public static SaveStateScript instance;

    private string saveDataPath;
    private string settingsDataPath;
    public GameObject mario = null;
    public bool isLoading=false;
    private bool marioload = false;
    private PlayerDataset gameData;


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
        mario = null;
        marioload = false;
        saveDataPath = Application.persistentDataPath + "/data.vgd";
        settingsDataPath = Application.persistentDataPath + "/settings.vgd";
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }



    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene is : " + scene.name);
        if (scene.name == "Menu" && mario != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Destroy(mario); 
            mario = null;
            return;
        }

        Debug.Log("Controller Loaded Into New Scene, checking Mario");
        checkMario();
        if (marioload)
        {
            marioload = false;
            StartCoroutine(waitLoadingAndLoadPlayer());
        }
        else
        {
            
            StartCoroutine(waitLoadingAndSave());
           
        }
    }

    IEnumerator waitLoadingAndSave()
    {
        yield return null;
        savePlayer();
    }
    IEnumerator waitLoadingAndLoadPlayer()
    {
        yield return null;
        gameData.loadToPlayer(mario.GetComponent<PlayerController>());
    }

    public void loadLevel(int level)
    {
        Time.timeScale = 1f;
        if (mario != null) 
        { 
            mario.transform.position = new Vector3(mario.transform.position.x, mario.transform.position.y + 1000f, mario.transform.position.z); 
            mario.GetComponent<PlayerController>().isImmune = true;
        }
        
        StartCoroutine(loadingTimer());
        
        PlayerPrefs.SetInt("CurrentLevel", (level) % SceneManager.sceneCountInBuildSettings);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));

        
    }
    public void nextLevel()
    {
        loadLevel(PlayerPrefs.GetInt("CurrentLevel") + 1);
    }

    IEnumerator loadingTimer()
    {
        isLoading = true;
        
        yield return new WaitForSecondsRealtime(3);
        isLoading = false;
    }

    public void resetToMenu()
    {
        PlayerPrefs.SetInt("CurrentLevel", 0);
        SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void prevLevel()
    {
        loadLevel(PlayerPrefs.GetInt("CurrentLevel") - 1);
    }
    public void checkMario()
    {
        if (mario==null)//Get the Mario that will stay alive
        {
            mario = GameObject.FindGameObjectWithTag("Player");
            if (mario != null)
            {
               
                DontDestroyOnLoad(mario);
            }
            else
                Debug.Log("Mario Not Attached, Coundn't find object with tag Player");
            return;
        }
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))//When moving into new scene delete the Mario of that scene if mario is already set
        {
            
            if (player != null && player != mario)
            {
                mario.GetComponent<PlayerController>().isImmune = true;
                Debug.Log("Found Scene Mario To Destroy, Replacing Coords");
                mario.transform.rotation = player.transform.rotation;
                mario.GetComponent<PlayerController>().startingPos = player.transform.position;
                mario.transform.position = mario.GetComponent<PlayerController>().startingPos;
                mario.GetComponent<PlayerController>().canvasScript = player.GetComponent<PlayerController>().canvasScript;
                mario.GetComponent<PlayerController>().isImmune = false;
                Destroy(player);
            }
        }
    }

    void savePlayer()
    {
        
        Debug.Log(saveDataPath);
        //Vector3 position = transform.position; //versione posizione semplice
        PlayerDataset gameData = new PlayerDataset(instance.mario.GetComponent<PlayerController>());  //versione dati di gioco


       
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.OpenOrCreate);
        
        //formatter.Serialize(fileStream, position); //versione posizione semplice
        formatter.Serialize(fileStream, gameData); //versione dati di gioco
        fileStream.Close();
        
    }

    public bool loadPlayer()
    {
        Debug.Log("load");
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveDataPath, FileMode.Open);

            gameData = (PlayerDataset)formatter.Deserialize(fileStream);

            PlayerPrefs.SetInt("CurrentLevel", gameData.level);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
            marioload = true;
            
            
            fileStream.Close();
            return true;
        }
        return false;
    }

    public bool loadSettings()
    {
        Debug.Log("loading Settings");
        if (File.Exists(settingsDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(settingsDataPath, FileMode.Open);

            SettingsData settingsData = (SettingsData)formatter.Deserialize(fileStream);

            fileStream.Close();
            settingsData.loadToGame(SettingsScript.instance);
            return true;
        }
        return false;
    }
    public void saveSettings()
    {
        Debug.Log("saving settings @ -> " + settingsDataPath);
        SettingsData settingsData = new SettingsData(SettingsScript.instance);

        BinaryFormatter formatter = new BinaryFormatter();

        using(FileStream fileStream = File.Open(settingsDataPath, FileMode.OpenOrCreate))
        {
            formatter.Serialize(fileStream, settingsData);
            fileStream.Close();
        }   
        
    }
}
