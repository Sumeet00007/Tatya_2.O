using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtifactSpawner : MonoBehaviour
{
    public GameObject[] task1Artifacts; // Lemon, Mirchi, Thread
    public GameObject[] task2Artifacts; // Voodoo Doll, Pins
    public GameObject[] task3Artifacts; // Talisman, Thread
    public GameObject[] falseArtifacts; // False artifacts pool

    private int currentTask = 0;
    public float spawnTimer = 3.0f;
    private bool isSpawning = false;
    private List<GameObject> spawnQueue = new List<GameObject>();

    private GameObject lastSpawnedArtifact = null;
    public AudioSource conveyorfx;
    public AudioClip conveyorbeltdestroysfx;
    private void Start()
    {
       //StartArtifactSpawning();
       StopArtifactSpawning(false);
       currentTask = 0;
       conveyorfx = GetComponent<AudioSource>();    
      
    }

    public void StartArtifactSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            PrepareSpawnQueue();
            StartCoroutine(SpawnArtifactsRoutine());
        }
    }

    private IEnumerator SpawnArtifactsRoutine()
    {
        while (isSpawning)
        {
            if (spawnQueue.Count == 0)
            {
                PrepareSpawnQueue(); // Refill the queue to keep spawning indefinitely
            }

            SpawnArtifact();
            yield return new WaitForSeconds(spawnTimer);
        }
    }

    private void PrepareSpawnQueue()
    {
        spawnQueue.Clear();

        if (currentTask == 0)
        {
            AddArtifactsToQueue(task1Artifacts, 3, 4);
        }
        else if (currentTask == 1)
        {
            AddArtifactsToQueue(task2Artifacts, 2, 3);
        }
        else if (currentTask == 2)
        {
            AddArtifactsToQueue(task3Artifacts, 2, 5);
        }
        else
        {
            StopArtifactSpawning(true); // Disable after third task
        }
    }

    private void AddArtifactsToQueue(GameObject[] artifacts, int correctCount, int falseCount)
    {
        if (artifacts.Length == 0 || falseArtifacts.Length == 0) return;

        // Add correct artifacts
        for (int i = 0; i < correctCount; i++)
        {
            spawnQueue.Add(artifacts[Random.Range(0, artifacts.Length)]);
        }

        // Add false artifacts
        for (int i = 0; i < falseCount; i++)
        {
            spawnQueue.Add(falseArtifacts[Random.Range(0, falseArtifacts.Length)]);
        }

        // Shuffle the spawn queue
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            GameObject temp = spawnQueue[i];
            int randomIndex = Random.Range(i, spawnQueue.Count);
            spawnQueue[i] = spawnQueue[randomIndex];
            spawnQueue[randomIndex] = temp;
        }
    }

    private void SpawnArtifact()
    {
        if (spawnQueue.Count == 0) return;

        int spawnIndex = -1;

        // Try to find a different artifact from the last spawned one
        for (int i = 0; i < spawnQueue.Count; i++)
        {
            if (spawnQueue[i] != lastSpawnedArtifact)
            {
                spawnIndex = i;
                break;
            }
        }

        // If all are the same as last spawned, just use the first one
        if (spawnIndex == -1) spawnIndex = 0;

        GameObject selectedArtifact = spawnQueue[spawnIndex];
        Instantiate(selectedArtifact, transform.position, Quaternion.identity);
        lastSpawnedArtifact = selectedArtifact;
        spawnQueue.RemoveAt(spawnIndex);
    }

    //function responsible for changing artifacts
    public void CompleteTask()
    {
        currentTask++;
        if (currentTask > 2)
        {
            StopArtifactSpawning(true);
        }
        else
        {
            PrepareSpawnQueue(); // Reset queue for the next task
        }
    }


    //function Responsible for StopArtifactSpawning when task is completed
    public void StopArtifactSpawning(bool finalStop)
    {
        isSpawning = false;
        StopAllCoroutines();

        if (finalStop)
        {
            Debug.Log("All tasks completed. Artifact spawner stopped permanently.");
            // Remove or comment out this line:
            gameObject.SetActive(false); 
        }
        else
        {
            Debug.Log("Task completed. Artifact spawner stopped temporarily.");
        }
    }

    public void DestroyConveyorBelt()
    {
        Debug.Log("Conveyor belt is destroyed");
        conveyorfx.PlayOneShot(conveyorbeltdestroysfx);
    }

    public void StartConveyorbeltSound()
    {
        conveyorfx.Play();
    }

    public void StopConveyorbeltSound()
    {
        conveyorfx.Stop();
    }


}
