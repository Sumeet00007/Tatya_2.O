using UnityEngine;

public class PlatformScare : MonoBehaviour
{
    public AudioSource pianoScare;
    public BoxCollider soundTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider soundTrigger)
    {
        pianoScare.Play();
    }
}
