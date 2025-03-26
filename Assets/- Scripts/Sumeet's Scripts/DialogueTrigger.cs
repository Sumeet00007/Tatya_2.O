using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private Shadow parentScript;

    private void Start()
    {
        parentScript = GetComponentInParent<Shadow>(); // Get the Shadow script from the parent
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the player has the correct tag
        {
            parentScript.StartDialogue(); // Trigger dialogue from parent
            Destroy(gameObject); // Destroy the trigger to ensure it plays only once
        }
    }
}
