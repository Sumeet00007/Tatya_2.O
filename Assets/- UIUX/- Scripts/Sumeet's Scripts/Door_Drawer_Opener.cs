using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Door_Drawer_Opener : MonoBehaviour, IInteractable
{
    public Transform closedPosition; // Transform for closed position
    public Transform openPosition;   // Transform for open position
    public float moveSpeed = 2f;     // Speed of movement
    private bool isOpen = false;

    void Start()
    {
        if (closedPosition == null)
        {
            closedPosition = transform; // Default to current position if not assigned
        }
    }

    public void PlayerInteracted()
    {
        ToggleMovement();
    }

    void ToggleMovement()
    {
        Transform targetTransform;

        if (isOpen)
        {
            targetTransform = closedPosition;
        }
        else
        {
            targetTransform = openPosition;
        }

        StopAllCoroutines();
        StartCoroutine(MoveObject(targetTransform));
        isOpen = !isOpen;
    }

    IEnumerator MoveObject(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * moveSpeed);
            yield return null;
        }
        transform.position = target.position; // Ensure exact position
    }
}
