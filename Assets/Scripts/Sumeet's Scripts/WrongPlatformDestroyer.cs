using UnityEngine;

public class WrongPlatformDestroyer : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 4.0f; // Time before platform gets destroyed

    private bool isPlayerOnPlatform = false; // Track if player is on platform
    private float timer = 0f; // Timer to track how long player stays

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && CompareTag("wrongPlatform"))
        {
            if (!isPlayerOnPlatform)
            {
                isPlayerOnPlatform = true; // Mark player as on the platform
                timer = 0f; // Reset the timer
            }

            timer += Time.deltaTime; // Increase timer while player stays

            if (timer >= destroyDelay) // Destroy platform after delay
            {
                //add condition to destroy/Boom & also connect to damage script of player if exhist
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false; // Reset when player leaves
            timer = 0f;
        }
    }
}
