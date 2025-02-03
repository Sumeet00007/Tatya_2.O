//using UnityEngine;

//public class DrawerSlide : MonoBehaviour
//{
//    [Header("Slide Settings")]
//    [SerializeField] float slideDistance = 2f;  // Distance the drawer will slide in/out
//    [SerializeField] float slideSpeed = 2f;     // Speed of the sliding
//    [SerializeField] bool isSlidingIn = true;   // Determine if the drawer is currently sliding in or out

//    [Header("Player Settings")]
//    [SerializeField] Transform player;  // Reference to the player
//    [SerializeField] float activationRange = 5f;  // Range at which the drawer can be interacted with
//    [SerializeField] Camera playerCamera; // Player's camera for raycasting

//    private bool canSlide = false;
//    private bool isLookingAtDrawer = false;
//    private bool isSliding = false; // Track if the drawer is sliding

//    private Vector3 initialPosition;
//    private Vector3 targetPosition;

//    void Start()
//    {
//        initialPosition = transform.position;
//        targetPosition = initialPosition + transform.forward * slideDistance; // Set target position based on slide distance
//    }

//    void Update()
//    {
//        // Check if the player is within range
//        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
//        canSlide = distanceToPlayer <= activationRange;

//        // Check if player's crosshair is pointing at the drawer using raycast
//        RaycastHit hit;
//        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); // Ray from the center of the screen
//        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
//        {
//            isLookingAtDrawer = hit.transform == transform;
//        }
//        else
//        {
//            isLookingAtDrawer = false;
//        }

//        // Slide the drawer when 'E' is pressed, within range, and the crosshair is on the drawer
//        if (canSlide && isLookingAtDrawer && Input.GetKeyDown(KeyCode.E) && !isSliding)
//        {
//            StartCoroutine(SlideDrawerSmoothly());
//        }
//    }

//    System.Collections.IEnumerator SlideDrawerSmoothly()
//    {
//        isSliding = true;

//        Vector3 targetPos = isSlidingIn ? targetPosition : initialPosition; // Choose target position based on direction

//        // Smoothly slide the drawer to the target position
//        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
//        {
//            transform.position = Vector3.MoveTowards(transform.position, targetPos, slideSpeed * Time.deltaTime);
//            yield return null;
//        }

//        // Set position exactly at the target position
//        transform.position = targetPos;

//        // Toggle sliding direction for the next time (slide in if it's out, or slide out if it's in)
//        isSlidingIn = !isSlidingIn;

//        // Allow interaction again after sliding is complete
//        isSliding = false;
//    }
//}

using UnityEngine;
using System.Collections;

public class DrawerSlide : MonoBehaviour
{
    [Header("Slide Settings")]
    [SerializeField] float slideDistance = 2f;
    [SerializeField] float slideSpeed = 2f;
    [SerializeField] bool isSlidingIn = true;

    [Header("Player Settings")]
    [SerializeField] Transform player;
    [SerializeField] float activationRange = 5f;
    [SerializeField] Camera playerCamera;

    private bool canSlide = false;
    private bool isLookingAtDrawer = false;
    private bool isSliding = false;
    private bool isLocked = true; // The drawer starts locked

    private Vector3 initialPosition;
    private Vector3 targetPosition;

    void Start()
    {
        initialPosition = transform.position;
        targetPosition = initialPosition + transform.forward * slideDistance;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);
        canSlide = distanceToPlayer <= activationRange;

        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            isLookingAtDrawer = hit.transform == transform;
        }
        else
        {
            isLookingAtDrawer = false;
        }

        if (!isLocked && canSlide && isLookingAtDrawer && Input.GetKeyDown(KeyCode.E) && !isSliding)
        {
            StartCoroutine(SlideDrawerSmoothly());
        }
    }

    public void UnlockDrawer()
    {
        isLocked = false;
        StartCoroutine(SlideDrawerSmoothly()); // Open automatically when unlocked
    }

    IEnumerator SlideDrawerSmoothly()
    {
        isSliding = true;

        Vector3 targetPos = isSlidingIn ? targetPosition : initialPosition;

        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, slideSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPos;
        isSlidingIn = !isSlidingIn;
        isSliding = false;
    }
}
