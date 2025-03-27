using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class BasementDoor : MonoBehaviour
{
    public enum DoorType { Door, Drawer }
    public DoorType doorType;

    public Transform hinge;
    public Transform closedPosition;
    public Transform openPosition;
    public float openAngle = 90f;
    public float moveSpeed = 2f;
    public bool isOpen = false;
    public bool isLocked = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    public UnityEvent onActionTriggered;
    public float jumpScareDelay = 1.0f;
    public AudioSource doorOpenSound;
    public AudioSource doorCloseSound;
    private BasementDoor basementdoor;

    private bool canInteract = false;

    //UI component to display UI when door is locked
    //public TMP_Text doorLockedMessage; // TextMeshPro text reference
    //public float messageDuration = 1.5f;

    //To control sound
    private bool isScriptEnabled;

    void Start()
    {
        if (doorType == DoorType.Door)
        {
            if (hinge == null)
            {
                hinge = transform;
            }
            closedRotation = hinge.rotation;
            openRotation = hinge.rotation * Quaternion.Euler(0, openAngle, 0);
        }

        basementdoor = GetComponent<BasementDoor>();
        //DisableScript();
        //EnableScript();

        //if (doorLockedMessage != null)
        //{
        //    doorLockedMessage.gameObject.SetActive(false); // Hide initially
        //}
    }

    void Update()
    {
        if (canInteract && Input.GetKeyDown(KeyCode.E)) // Example for "E" key interaction
        {
            PlayerInteracted();
        }
    }

    public void PlayerInteracted()
    {
        if (!canInteract || isLocked)
        {
            //if (doorLockedMessage != null)
            //{
            //    StartCoroutine(ShowLockedMessage());
            //}
            return;
        }

        if (isOpen)
        {
            if (doorCloseSound != null && isScriptEnabled==true)
            {
                doorCloseSound.Play();
            }
            CloseDoorOrDrawer();
        }
        else
        {
            if (doorOpenSound != null && isScriptEnabled==true)
            {
                doorOpenSound.Play();
            }
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
        hinge.rotation = targetRotation;
    }

    IEnumerator SlideObject(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target.position;
    }

    public void TriggerEvent()
    {
        if (onActionTriggered != null)
        {
            onActionTriggered.Invoke();
        }
    }

    public void EnableScript()
    {
        if (basementdoor != null)
        {
            Debug.Log("Doors are opened");
            basementdoor.enabled = true;
            canInteract = true;
            isScriptEnabled = true;
        }
    }

    public void DisableScript()
    {
        if (basementdoor != null)
        {
            basementdoor.enabled = false;
            canInteract = false;
            isScriptEnabled = false;
        }
    }

    //IEnumerator ShowLockedMessage()
    //{
    //    if (doorLockedMessage != null)
    //    {
    //        doorLockedMessage.text = "Door is Locked";
    //        doorLockedMessage.gameObject.SetActive(true);
    //        yield return new WaitForSeconds(messageDuration);
    //        doorLockedMessage.gameObject.SetActive(false);
    //    }
    //}
}
