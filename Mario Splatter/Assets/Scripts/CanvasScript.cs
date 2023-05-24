using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CanvasScript : MonoBehaviour
{
    public static bool gunUpdate = true;
    public static int currentGun = 0;
    public static int scoreValue = 0;
    TextMeshProUGUI score;
    Transform[] crossAires;
    // Start is called before the first frame update
    void Start()
    {
        score = transform.Find("Score").GetComponent<TextMeshProUGUI>();
        Transform[] ca= {transform.Find("AssaultRifleAim"),
            transform.Find("ShotgunAim"),
            transform.Find("PistolAim") };
        crossAires = ca;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       score.SetText("" + scoreValue);
       if (gunUpdate)
            {
            updateCrossAir(crossAires, currentGun);
            gunUpdate = false;
            }
        
    }
    void updateCrossAir(Transform[] crossAires, int gun)
    {
        int i = 0;
        foreach (Transform ca in crossAires)
        {
            if (i == gun)
                ca.gameObject.SetActive(true);
            else
                ca.gameObject.SetActive(false);
            i++;
        }
    }
}

