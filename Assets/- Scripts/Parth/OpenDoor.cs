using UnityEngine;

public class OpenDoor : MonoBehaviour, ICompletionHandler
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] DoorOpener doorOpener;

    public void OnCompletion()
    {
        doorOpener.isOpen = true;
    }
}
