using UnityEngine;

public class ZombieJumpScare : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;
    private bool hasSpawned = false;

    //reference for audioSource

    public void EnableZombieJumpScare()
    {
        if (!hasSpawned && zombiePrefab != null)
        {
            //Play audioFX of zombie Crawling

            GameObject skull = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            hasSpawned = true;
        }
    }
}
