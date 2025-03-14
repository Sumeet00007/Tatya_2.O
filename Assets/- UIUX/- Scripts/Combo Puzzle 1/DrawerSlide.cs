

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

        if (!isLocked && canSlide && isLookingAtDrawer && /*Input.GetKeyDown(KeyCode.E)*/ Input.GetKeyDown(KeyCode.Mouse0)  && !isSliding)
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
