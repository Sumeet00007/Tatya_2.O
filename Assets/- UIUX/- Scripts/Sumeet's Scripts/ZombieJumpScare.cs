using UnityEngine;

public class ZombieJumpScare : MonoBehaviour
{
    public GameObject zombiePrefab;
    public Transform spawnPoint;
    private bool hasSpawned = false;

    public void EnableZombieJumpScare()
    {
        if (!hasSpawned && zombiePrefab != null)
        {
            GameObject skull = Instantiate(zombiePrefab, spawnPoint.position, spawnPoint.rotation);
            hasSpawned = true;
        }
    }
}
