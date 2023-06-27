using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BooController : EnemyController
{
    private Coroutine currentCoroutine = null;

    [Header("Boo Stats")]

    [SerializeField] public float booSpeed = 0.1f;
    [SerializeField] public float revisibleTime = 0.5f;
    [SerializeField] public bool isVisible = false;

    [Header("Boo Sounds")]
    [SerializeField] AudioClip booFollowSound;
    [SerializeField] AudioClip booStopSound;

    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0 && !isDead)
        {
            death();
            player.GetComponent<PlayerController>().score += enemyScore;
        }
        if (!isVisible)
        {
            Vector3 origin = transform.position;
            Vector3 target = player.transform.position;



            Vector3 direction = (target - origin).normalized;

            if(direction != Vector3.zero) transform.forward = direction;

            transform.position = Vector3.Lerp(origin, target, booSpeed);
        }

    }

    public void setSkinIsCover(bool value)
    {
        transform.Find("EyeCover").gameObject.SetActive(value);
        transform.Find("HandCover").gameObject.SetActive(value);
        transform.Find("EyeChase").gameObject.SetActive(!value);
        transform.Find("HandChase").gameObject.SetActive(!value);
    }

    private IEnumerator revisibleTimer()
    {
        
        yield return new WaitForSeconds(booStopSound.length);
        

    }

    private IEnumerator invisibleTimer()
    {
        Debug.Log("Non mi vedi");
        setSkinIsCover(false);
        audioSource.PlayOneShot(booFollowSound);
        
        yield return new WaitForSeconds(booFollowSound.length);
        if (isVisible)
            isVisible = false;

    }

    public IEnumerator SetFollowing(bool value)
    {
        
        Debug.Log("coroutine started");
        if (value)
        {
            
            setSkinIsCover(false);
            audioSource.PlayOneShot(booFollowSound);
            yield return new WaitForSeconds(booFollowSound.length);
            Debug.Log("Non mi vedi");
            isVisible = false;
        }
        else
        {
            Debug.Log("Setting visible");
            isVisible = true;
            setSkinIsCover(true);
            audioSource.PlayOneShot(booStopSound);
        }
    }

    private void OnBecameVisible()
    {
        if(currentCoroutine!=null)
            StopCoroutine(currentCoroutine);
        if (gameObject.activeInHierarchy)
            currentCoroutine = StartCoroutine(SetFollowing(false));
    }



    private void OnBecameInvisible()
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);
        if(gameObject.activeInHierarchy)
            currentCoroutine = StartCoroutine(SetFollowing(true));
    }

}
