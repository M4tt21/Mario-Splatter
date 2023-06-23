using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDespawnerScript : MonoBehaviour
{
    public BossArenaControllerScript arenaController;
    private void OnTriggerExit(Collider other)
    {
        arenaController.despawnBoss(other);
    }
}
