using UnityEngine;

public class Painting : MonoBehaviour, IInteractable
{
    Rigidbody rb;
    public AudioSource playerSource;
    public AudioClip paintingSound; 

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void PlayerInteracted()
    {
        rb.isKinematic = false;
        playerSource.PlayOneShot(paintingSound);
        Debug.Log("Player Interacted");
    }
}
