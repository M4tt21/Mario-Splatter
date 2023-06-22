using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeachController : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hai Vinto!!!!!");
        }
    }
}
