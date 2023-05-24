using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopaController : EnemyController
{
    private float KoopaHealth = 200;
    // Start is called before the first frame update
    void Start()
    {
        initHealth(KoopaHealth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(health<=0)
        {
            death(); 
            CanvasScript.scoreValue += 1;
        }
    }
}
