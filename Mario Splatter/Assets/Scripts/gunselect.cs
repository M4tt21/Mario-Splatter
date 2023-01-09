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
        int previousSelecterGun = selectedGun;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedGun = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedGun = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedGun = 2;
        if (previousSelecterGun != selectedGun)
            SelectGun();
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
