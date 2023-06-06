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


    // Start is called before the first frame update
    void Start()
    {
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
        saveDataPath = Application.persistentDataPath + "/data.vgd";

        //Vector3 position = transform.position; //versione posizione semplice
        GameData gameData = new GameData();  //versione dati di gioco
        gameData.position = new SerializableVector3(this.transform.position);


        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Open(saveDataPath, FileMode.Create);

        //formatter.Serialize(fileStream, position); //versione posizione semplice
        formatter.Serialize(fileStream, gameData); //versione dati di gioco
    }

    void load()
    {
        if (File.Exists(saveDataPath))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(saveDataPath, FileMode.Open);

            GameData gameData = (GameData)formatter.Deserialize(fileStream);

            transform.position = gameData.position.toVector3();
        }
    }
}
