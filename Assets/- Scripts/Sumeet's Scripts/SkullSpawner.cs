using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkullSpawner : MonoBehaviour
{
    public GameObject zombieSkullPrefab;
    //give reference to audioSource

    public Transform spawnPoint;
    public float fallVelocity = -5f;
    private bool hasSpawned = false; 

  

    public void SpawnZombieSkull()
    {
        if (!hasSpawned && zombieSkullPrefab != null)
        {
            //play audiofx for falling zombieSkull 

            GameObject skull = Instantiate(zombieSkullPrefab, spawnPoint.position, spawnPoint.rotation);

            Rigidbody rb= skull.GetComponent<Rigidbody>();

            if(rb != null )
            {
                rb.linearVelocity = new Vector3(0, fallVelocity, 0);
            }


            hasSpawned = true;
        }
    }
}
