using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QMBlockScript : MonoBehaviour
{
    public GameObject pickUp;
    public bool isCollected = false;
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isCollected)
        {
            audio.Play();
            StartCoroutine(DisapearWaitTime());
            DropPickUp();

            isCollected = true;
            
        }
    }
    public void DropPickUp()
    {
        GameObject drop = Instantiate(pickUp).gameObject;
        drop.transform.position = transform.position;
    }
    IEnumerator DisapearWaitTime()
    {

        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
