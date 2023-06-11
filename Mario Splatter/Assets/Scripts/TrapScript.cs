using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    [Header("General Trap Stats")]
    [SerializeField] public float despawnTime = 5f;
    [SerializeField] public float damageToPlayer = 10f;

    private void Awake()
    {
        StartCoroutine(despawnTimer());
    }

    private IEnumerator despawnTimer()
    {
        yield return new WaitForSeconds(despawnTime);
        Destroy(gameObject);
    }
}
