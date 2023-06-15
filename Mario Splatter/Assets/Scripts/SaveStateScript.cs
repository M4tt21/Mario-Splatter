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
    private bool marioload = false;
    private PlayerDataset gameData;


    // Start is called before the first frame update
    void Start()
    {

        Debug.Log("SONO STARTATO;");

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
        if (scene.name == "menu" && mario != null)
        {
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
            Debug.Log("WOOOW COGLIONE");
            StartCoroutine(waitLoadingAndSave());
            Debug.Log("WOOOW COGLIONE 2");
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


    // Update is called once per frame
    void Update()
    {
        //serve se vogliamo farlo usando degli input tastiera
        /*if (Input.GetKeyDown("p"))
        {
            Debug.Log("Quick save");
            save();
        }
        if (Input.GetKeyDown("o"))
        {
            load();
            Debug.Log("Quick Load");
        }*/
    }


    public void checkMario()
    {
        if (mario==null)//Get the Mario that will stay alive
        {
            mario = GameObject.FindGameObjectWithTag("Player");
            if (mario != null)
            {
                Debug.Log("Mario Attached");
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
                Debug.Log("Found Scene Mario To Destroy, Replacing Coords");
                mario.transform.position = player.transform.position;
                mario.transform.rotation = player.transform.rotation;
                mario.GetComponent<PlayerController>().startingPos = player.transform.position;
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


        Debug.Log("WOOOW 1");
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.OpenOrCreate);
        Debug.Log("WOOOW 2");
        //formatter.Serialize(fileStream, position); //versione posizione semplice
        formatter.Serialize(fileStream, gameData); //versione dati di gioco
        fileStream.Close();
        Debug.Log("WOOOW 3");
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
