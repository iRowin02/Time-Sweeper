using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnHandler : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject toSpawn;
    private bool hasDone = false;

    public void OnTriggerEnter(Collider other)
    {
        if (hasDone == true)
            return;
        if(other.gameObject.tag == "Player")
        {
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                Instantiate(toSpawn, spawnPoints[i].transform.position, Quaternion.identity);
            }
            hasDone = true;
        }
    }
}
