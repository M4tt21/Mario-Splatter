using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BooController : EnemyController
{

    [Header("Boo Stats")]

    [SerializeField] public float booSpeed = 0.1f;
    [SerializeField] public float revisibleTime = 0.5f;
    [SerializeField] public bool isVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        initHealth(health);
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        animator = gameObject.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
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
        yield return new WaitForSeconds(revisibleTime);
        Debug.Log("Mi vedi");
        isVisible = true;
        setSkinIsCover(true);
    }

    private void OnBecameVisible()
    {
        StartCoroutine(revisibleTimer());
    }
    private void OnBecameInvisible()
    {
        Debug.Log("Non mi vedi");
        isVisible = false;
        setSkinIsCover(false);
    }

}
