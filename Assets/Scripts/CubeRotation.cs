//using UnityEngine;

//public class CubeRotation : MonoBehaviour
//{
//    [Header("Rotation Settings")]
//    [SerializeField] float rotationSpeed = 50f;  // Rotation speed
//    [SerializeField] float rotationAngle = 90f; // Rotation angle (90 degrees)
//    [SerializeField] float smoothRotationSpeed = 1f; // Smooth rotation speed (slower rotation)

//    [Header("Player Settings")]
//    [SerializeField] Transform player;  // Reference to the player
//    [SerializeField] float activationRange = 5f;  // Range at which the cube rotates
//    [SerializeField] Camera playerCamera; // Player's camera for raycasting

//    private bool canRotate = false;
//    private bool isLookingAtCube = false;
//    private bool isRotating = false; // Track if the cube is rotating
//    private float currentRotationZ = 0f;

//    void Update()
//    {
//        // Check if the player is within range
//        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
//        canRotate = distanceToPlayer <= activationRange;

//        // Check if player's crosshair is pointing at the cube using raycast
//        RaycastHit hit;
//        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); // Ray from the center of the screen
//        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
//        {
//            isLookingAtCube = hit.transform == transform;
//        }
//        else
//        {
//            isLookingAtCube = false;
//        }

//        // Rotate the cube when 'E' is pressed, within range, and the crosshair is on the cube
//        if (canRotate && isLookingAtCube && Input.GetKeyDown(KeyCode.E) && !isRotating)
//        {
//            StartCoroutine(RotateCubeSmoothly());
//        }
//    }

//    System.Collections.IEnumerator RotateCubeSmoothly()
//    {
//        isRotating = true;

//        float targetRotation = currentRotationZ + rotationAngle; // Target rotation (90 degrees from current)

//        // Smoothly rotate the cube
//        while (Mathf.Abs(currentRotationZ - targetRotation) > 0.1f)
//        {
//            currentRotationZ = Mathf.MoveTowards(currentRotationZ, targetRotation, rotationSpeed * Time.deltaTime);
//            transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ);
//            yield return null;
//        }

//        // Make sure it ends exactly at the target rotation
//        currentRotationZ = targetRotation;
//        transform.rotation = Quaternion.Euler(0f, 0f, currentRotationZ);

//        // Allow interaction again after rotation is complete
//        isRotating = false;
//    }
//}

using UnityEngine;
using System; // For Action event

public class CubeRotation : MonoBehaviour
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

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        canRotate = distanceToPlayer <= activationRange;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            isLookingAtCube = hit.transform == transform;
        }
        else
        {
            isLookingAtCube = false;
        }

        if (canRotate && isLookingAtCube && Input.GetKeyDown(KeyCode.E) && !isRotating)
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
