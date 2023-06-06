using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragoneelController : EnemyController
{
    [Header("Dragoneel Info")]
    public float jumpCD=5f;
    [Range(1f, 100f)]
    public float jumpDistance = 10f;
    private bool isOnCD = false;
    private bool forward = true;

    // Update is called once per frame
    void Update()
    {
        if (health<=0 && !isDead)
        {
            death();
        }
        if (isOnCD)
            return;
        animator.Play("Long Jump", 0);
        StartCoroutine(jumpCDTime());
    }

    IEnumerator jumpCDTime()
    {
        isOnCD = true;
        yield return new WaitForSeconds(jumpCD);
        transform.eulerAngles = transform.eulerAngles + 180f * Vector3.up;
        isOnCD = false;
    }
}
