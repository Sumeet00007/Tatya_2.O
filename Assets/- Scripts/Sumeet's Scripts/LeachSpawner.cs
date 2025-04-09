using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LeachSpawner : MonoBehaviour
{
    public GameObject leachprefab;
    public Transform leachSpawnPoint;
    public float moveSpeed;
    public bool isSpawned = false;

    private GameObject spawnedLeach;
    public Transform player;
    public AudioSource jumpscarefx;


    void Start()
    {
        if (spawnedLeach == null)
        {
            return;
        }

        if(player ==null)
        {
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawned && spawnedLeach != null)
        {
            spawnedLeach.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            float distance = Vector3.Distance(spawnedLeach.transform.position, player.transform.position);
            //  Debug.Log(distance);    
            if (distance > 300.0f)
            {
                Destroy(spawnedLeach);
                //Debug.Log("Destroyed");
            }
        }


    }
    public void SpawnLeach()
    {
        if (!isSpawned)
        {
            jumpscarefx.Play();
            //Debug.Log("Spawned leach");
            spawnedLeach = Instantiate(leachprefab, leachSpawnPoint.position, leachSpawnPoint.rotation);
            isSpawned = true;
           
        }
    }

 
}
