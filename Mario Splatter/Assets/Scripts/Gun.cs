using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

        RaycastHit hitPoint;

        if (Input.GetMouseButton(0))//tasto sinistro mouse
        {
            bool raycastRes = Physics.Raycast(transform.position, transform.forward, out (hitPoint), Mathf.Infinity);
            if (raycastRes)
            {
                //Debug.Log("Collision at: " + hitPoint.distance); //traiettoria proiettile debug
                Debug.DrawRay(transform.position, transform.forward * 1000, Color.red); // traiettoria proiettile visibile
                if (!hitPoint.transform.CompareTag("floor"))//se non spara al pavimento allora...
                {
                    Destroy(hitPoint.transform.gameObject);//distruggi l'oggetto colpito 
                }
                //Alternativa per distruggere i cloni
                /*if (hitPoint.transform.CompareTag("Clone"))
                {
                    Destroy(hitPoint.transform.gameObject);
                }*/
            }
        }
    }
}
