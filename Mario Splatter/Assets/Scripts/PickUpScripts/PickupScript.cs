using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    protected AudioSource audioSource;
    private float destroyTime = 3f;
    public bool isCollected=false;

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
