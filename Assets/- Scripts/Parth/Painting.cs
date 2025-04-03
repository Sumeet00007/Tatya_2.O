using UnityEngine;

public class Painting : MonoBehaviour, IInteractable
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void PlayerInteracted()
    {
        rb.isKinematic = false;
        Debug.Log("Player Interacted");
    }
}
