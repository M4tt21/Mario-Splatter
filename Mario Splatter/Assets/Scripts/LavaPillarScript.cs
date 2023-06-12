using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaPillarScript : TrapScript
{

    [Header("Lava Pillar Stats")]
    public float hitBoxEnableDelay = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        transform.GetComponent<Collider>().enabled = false;
    }

    private void OnDisable()
    {
        transform.GetComponent<Collider>().enabled = false;
    }

    private IEnumerator hitBoxEnableDelayTimer()
    {
        yield return new WaitForSeconds(hitBoxEnableDelay);
        transform.GetComponent<Collider>().enabled = false;
    }
}
