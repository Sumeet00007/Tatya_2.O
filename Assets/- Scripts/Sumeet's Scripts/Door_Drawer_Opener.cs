using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DoorOpener : MonoBehaviour, IInteractable
{
    public enum DoorType { Door, Drawer }
    public DoorType doorType; // Determines whether it's a Door or Drawer

    public Transform hinge; // The hinge point of the door
    public Transform closedPosition; // Transform for closed position
    public Transform openPosition;   // Transform for open position
    public float openAngle = 90f; // The angle to open the door
    public float moveSpeed = 2f; // Speed of movement
    public bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;


    public UnityEvent onActionTriggered;
<<<<<<< Updated upstream
    public float jumpScareDelay=1.0f;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;
=======
    public float jumpScareDelay = 1.0f;
>>>>>>> Stashed changes

    void Start()
    {
        if (doorType == DoorType.Door)
        {
            if (hinge == null)
            {
                hinge = transform; // Default to self if no hinge is assignedd
            }
            closedRotation = hinge.rotation;
            openRotation = hinge.rotation * Quaternion.Euler(0, openAngle, 0);
        }
    }

    public void PlayerInteracted()
    {
        if (isOpen == true)
        {
            doorCloseSound.Play();
            CloseDoorOrDrawer();
        }
        else
        {
            doorOpenSound.Play();
            OpenDoorOrDrawer();
        }
    }

    void OpenDoorOrDrawer()
    {
        StopAllCoroutines();

        if (doorType == DoorType.Drawer)
        {
            StartCoroutine(SlideObject(openPosition));
        }
        else if (doorType == DoorType.Door)
        {
            if (closedPosition != null && openPosition != null)
            {
                StartCoroutine(SlideObject(openPosition));
            }
            else
            {
                StartCoroutine(RotateDoor(openRotation));
            }
        }

        isOpen = true;
        Invoke(nameof(TriggerEvent), jumpScareDelay);
    }

    void CloseDoorOrDrawer()
    {
        StopAllCoroutines();

        if (doorType == DoorType.Drawer)
        {
            StartCoroutine(SlideObject(closedPosition));
        }
        else if (doorType == DoorType.Door)
        {
            if (closedPosition != null && openPosition != null)
            {
                StartCoroutine(SlideObject(closedPosition));
            }
            else
            {
                StartCoroutine(RotateDoor(closedRotation));
            }
        }

        isOpen = false;
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Lerp(hinge.rotation, targetRotation, Time.deltaTime * moveSpeed);
            yield return null;
        }
        hinge.rotation = targetRotation; // Ensure exact rotation
    }

    IEnumerator SlideObject(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target.position; // Ensure exact position
    }

    public void TriggerEvent()
    {
        if (onActionTriggered != null)
        {
            onActionTriggered.Invoke();
        }
    }
}
