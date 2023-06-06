using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragoneelController : EnemyController
{
    [Header("Dragoneel Info")]
    public float jumpCD=5f;
    [Range(1f, 100f)]
    private bool isOnCD = false;

    // Update is called once per frame
    void Update()
    {
        if (health<=0 && !isDead)
        {
            death();
        }
        else if (isOnCD || isDead)
            return;
        animator.Play("Long Jump", 0);
        StartCoroutine(jumpCDTime());
    }

    IEnumerator jumpCDTime()
    {
        isOnCD = true;
        yield return new WaitForSeconds(jumpCD);
        if(!isDead)
            transform.eulerAngles = transform.eulerAngles + 180f * Vector3.up;
        isOnCD = false;
    }
}
