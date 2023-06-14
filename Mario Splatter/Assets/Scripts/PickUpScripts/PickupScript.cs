using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    protected AudioSource audioSource;
    private float destroyTime = 3f;
    public bool isCollected=false;

    /* Parametri di movimento */
    private Vector3 rotation = new Vector3(0f, 1f, 0f); //Velocità di rotazione sull'asse y
    public float speed = 10f;                            //Velocità di movimento

    // Start is called before the first frame update
    void Start()
    {

        /// Funzione utile per capire dal log quello che sta succedendo
        Debug.Log("Il pickup sta girando");

    }

    private void FixedUpdate()
    {

        /// Applichiamo una rotazione costante (perché siamo nella fixed update) al nostro cubo utlizzando il vettore che abbiamo dichiarato prima ///
        transform.Rotate(rotation);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.gameObject.CompareTag("Player"))
        {
            pickUpAction(other.gameObject);
            StartCoroutine(soundAndDestroy());
            isCollected = true;
        }
    }

    protected virtual void pickUpAction(GameObject player)
    {
        Debug.Log("Pickup Action Not Found");
    }

    protected IEnumerator soundAndDestroy()
    {
        if(TryGetComponent<AudioSource>(out audioSource))
            audioSource.Play();
        else
            Debug.Log("Pickup Audio Not Found");
        //Disable the renderers, cant destroy the object, wait for the sound to play
        foreach (Renderer renderer in transform.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null)
            {
                renderer.enabled = false;
            }
        }
        yield return new WaitForSeconds(destroyTime);
        if (gameObject != null)
            Destroy(gameObject);
    }

}
