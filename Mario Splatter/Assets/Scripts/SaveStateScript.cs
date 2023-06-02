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



    // Start is called before the first frame update
    void Start()
    {
        

        //se trova un player duplicato lo distrugge, sì può accadere
        GameObject[] oldPlayers = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject oldPlayer in oldPlayers)
        {
            if (oldPlayers != null && oldPlayer != gameObject)
            {
                Debug.Log("Sono dentro");
                oldPlayer.transform.position = transform.position;
                oldPlayer.GetComponent<PlayerController>().startingPos = transform.position;
                Destroy(gameObject);
            }
        }
        Debug.Log("Non ho distrutto Mario");
        GameObject.DontDestroyOnLoad(gameObject);// impedisce la distruzione immediata dell'oggetto
    }

    // Update is called once per frame
    void Update()
    {
        //serve se vogliamo farlo usando degli input tastiera
        if (Input.GetKeyDown("p"))
        {
            Debug.Log("Ho salvato");
            save();
        }
        if (Input.GetKeyDown("o"))
        {
            load();
            Debug.Log("Ho caricato");
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
