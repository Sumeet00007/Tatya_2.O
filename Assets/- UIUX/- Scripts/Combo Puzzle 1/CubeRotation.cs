using UnityEngine;
using System; // For Action event

public class CubeRotation : MonoBehaviour, IInteractable
{
    [Header("Rotation Settings")]
    [SerializeField] float rotationSpeed = 50f;
    [SerializeField] float rotationAngle = 90f;
    [SerializeField] float smoothRotationSpeed = 1f;

    [Header("Player Settings")]
    [SerializeField] Transform player;
    [SerializeField] float activationRange = 5f;
    [SerializeField] Camera playerCamera;

    [Header("Puzzle Settings")]
    [SerializeField] float correctRotationZ = 0f; // The correct rotation to solve the puzzle

    private bool canRotate = false;
    private bool isLookingAtCube = false;
    private bool isRotating = false;
    private float currentRotationZ = 0f;

    public static event Action OnCubeRotated; // Event for notifying PuzzleManager

    public void PlayerInteracted()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateCubeSmoothly());
        }
    }

    System.Collections.IEnumerator RotateCubeSmoothly()
    {
        isRotating = true;

        float targetRotation = currentRotationZ + rotationAngle;

        while (Mathf.Abs(currentRotationZ - targetRotation) > 0.1f)
        {
            currentRotationZ = Mathf.MoveTowards(currentRotationZ, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ);
            yield return null;
        }

        currentRotationZ = targetRotation;
        transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ);

        isRotating = false;

        OnCubeRotated?.Invoke(); // Notify the PuzzleManager
    }

    public bool IsCorrectlyAligned()
    {
        return Mathf.Abs(Mathf.DeltaAngle(currentRotationZ, correctRotationZ)) < 1f;
    }
}
