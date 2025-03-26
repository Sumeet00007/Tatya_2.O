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

    private void Start()
    {
        //StartArtifactSpawning();
        //currentTask = 0;
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
            AddArtifactsToQueue(task2Artifacts, 2, 5);
        }
        else if (currentTask == 2)
        {
            AddArtifactsToQueue(task3Artifacts, 2, 6);
        }
        else
        {
            StopArtifactSpawning(); // Disable after third task
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
        if (spawnQueue.Count > 0)
        {
            Instantiate(spawnQueue[0], transform.position, Quaternion.identity);
            spawnQueue.RemoveAt(0);
        }
    }

    //function responsible for changing artifacts
    public void CompleteTask()
    {
        currentTask++;
        if (currentTask > 2)
        {
            StopArtifactSpawning();
        }
        else
        {
            PrepareSpawnQueue(); // Reset queue for the next task
        }
    }


    //function Responsible for StopArtifactSpawning when task is completed
    public void StopArtifactSpawning()
    {
        isSpawning = false;
        StopAllCoroutines();
        gameObject.SetActive(false); // Disabling script after completion
    }

    public void DestroyConveyorBelt()
    {
        Debug.Log("Conveyor belt is destroyed");
    }
}
