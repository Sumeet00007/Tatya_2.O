using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class DoorOpening : MonoBehaviour, IInteractable
{
    public enum DoorType { Door, Drawer }
    public DoorType doorType; // Determines whether it's a Door or Drawer

    public Transform hinge; // The hinge point of the door
  
    public float openAngle = 90f; // The angle to open the door
    public float moveSpeed = 2f; // Speed of movement
    public bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;


    public UnityEvent onActionTriggered;
    public float jumpScareDelay=1.0f;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;

    public bool hasKey=false;

    void Start()
    {
        hasKey = false;
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
        if (isOpen==true)
        {
         
            CloseDoorOrDrawer();
        }
        else
        {
            OpenDoorOrDrawer();
        }
    }

    void OpenDoorOrDrawer()
    {
        StopAllCoroutines();
        if (doorType == DoorType.Door && hasKey == true)
        {
               StartCoroutine(RotateDoor(openRotation));
               doorOpenSound.Play();
        }

        isOpen = true;
        Invoke(nameof(TriggerEvent),jumpScareDelay);
    }

    void CloseDoorOrDrawer()
    {
        StopAllCoroutines();

        
        if (doorType == DoorType.Door)
        {
             StartCoroutine(RotateDoor(closedRotation));
        }

        isOpen = false;
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        if(hasKey == true)
        {
            doorCloseSound.Play();
        }
      
        while (Quaternion.Angle(hinge.rotation, targetRotation) > 0.1f)
        {
            hinge.rotation = Quaternion.Lerp(hinge.rotation, targetRotation, Time.deltaTime * moveSpeed);
            yield return null;
        }
        hinge.rotation = targetRotation; // Ensure exact rotation
    }

    

    public void TriggerEvent()
    {
        if (onActionTriggered != null)
        {
            onActionTriggered.Invoke();
        }
    }
}
