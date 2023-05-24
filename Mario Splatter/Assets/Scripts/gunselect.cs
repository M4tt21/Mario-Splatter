using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSelect : MonoBehaviour
{
    public int selectedGun = 0;
    // Start is called before the first frame update
    void Start()
    {
        SelectGun();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedGun = selectedGun;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedGun = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedGun = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedGun = 2;
        if (previousSelectedGun != selectedGun)
        { 
            SelectGun();
            CanvasScript.gunUpdate = true;
            CanvasScript.currentGun = selectedGun;
        }
            
    }
    void SelectGun()
    {
        int i = 0;
        foreach(Transform Guns in transform)
        {
            if (i == selectedGun)
                Guns.gameObject.SetActive(true);
            else
                Guns.gameObject.SetActive(false);
            i++;
        }
    }
}
