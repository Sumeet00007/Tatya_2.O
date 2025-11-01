using UnityEngine;

public class OpenDoor : MonoBehaviour, ICompletionHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DoorOpener doorOpener;
    public AudioSource fuse2Source;
    public AudioClip fuseONSound;

    public void OnCompletion()
    {
        doorOpener.isLocked = false;
       fuse2Source.PlayOneShot(fuseONSound);
    }
}