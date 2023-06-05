using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    protected AudioSource audioSource;
    private float destroyTime = 1f;
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
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

}
