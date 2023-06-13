using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;



public class SaveStateScript : MonoBehaviour
{
    //salvataggio
    private string saveDataPath;
    private GameObject mario = null;
    private bool marioload = false;
    private PlayerDataset gameData;


    // Start is called before the first frame update
    void Start()
    {
        saveDataPath = Application.persistentDataPath + "/data.vgd";
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Awake()
    {
        Debug.Log("Controller Awake, checking Mario");
        checkMario();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Controller Loaded Into New Scene, checking Mario");
        checkMario();
        if (marioload)
        {
            marioload = false;
            gameData.loadToPlayer(mario.GetComponent<PlayerController>());
        }
        else 
            save();
    }


    // Update is called once per frame
    void Update()
    {
        //serve se vogliamo farlo usando degli input tastiera
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Quick save");
            save();
        }
        if (Input.GetKeyDown("o"))
        {
            load();
            Debug.Log("Quick Load");
        }
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
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player"))//When moving into new scene delete the Mario of that scene
        {
            
            if (player != null && player != mario)
            {
                Debug.Log("Found Scene Mario To Destroy, Replacing Coords");
                mario.transform.position = player.transform.position;
                mario.transform.rotation = player.transform.rotation;
                mario.GetComponent<PlayerController>().startingPos = player.transform.position;
                Destroy(player);
            }
        }
    }

    void save()
    {
        
        Debug.Log(saveDataPath);
        //Vector3 position = transform.position; //versione posizione semplice
        PlayerDataset gameData = new PlayerDataset(mario.GetComponent<PlayerController>());  //versione dati di gioco
        


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.Create);

        //formatter.Serialize(fileStream, position); //versione posizione semplice
        formatter.Serialize(fileStream, gameData); //versione dati di gioco
        fileStream.Close();
    }

    public void load()
    {
        Debug.Log("load");
        if (File.Exists(saveDataPath))
        {
            Debug.Log("file cagato");
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveDataPath, FileMode.Open);

            gameData = (PlayerDataset)formatter.Deserialize(fileStream);

            PlayerPrefs.SetInt("CurrentLevel", gameData.level);
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
            marioload = true;
            
            
            fileStream.Close();
            
        }
    }
}
