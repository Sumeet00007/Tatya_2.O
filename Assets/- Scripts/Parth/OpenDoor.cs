using UnityEngine;

public class OpenDoor : MonoBehaviour, ICompletionHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DoorOpener doorOpener;
    public AudioSource switchONSound2;

    public void OnCompletion()
    {
        doorOpener.isLocked = false;
        switchONSound2.Play();
    }
}