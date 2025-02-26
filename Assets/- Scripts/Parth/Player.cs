using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [Header("Mouse Look Variables")]

    [SerializeField] Camera playerCamera;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxLookUp, maxLookDown;

    float xRotation = 0f;

    [Header("Movement Variables")]

    [SerializeField] CharacterController playerController;
    [SerializeField] float moveSpeed = 5f;

    [Header("Jumping Variables")]

    [SerializeField] float jumpHeight;

    [Header("Gravity Variables")]

    [SerializeField] Transform groundCheckPoint;
    [SerializeField] LayerMask groundLayerMask;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] float checkSphereRadius = 0.4f;
    [SerializeField] float groundedDownVelocity = -2f;

    Vector3 velocity;
    bool isGrounded;

    [Header("Interactions Variables")]

    public Transform itemContainer;
    public bool isHandsFree;

    [SerializeField] float sphereCastRadius;
    [SerializeField] float sphereCastRange;
    [SerializeField] float sphereCastDeviation;
    [SerializeField] float dropForwardForce;
    [SerializeField] float dropUpwardForce;

    RaycastHit objectHit;
    Transform currentItem;
    Rigidbody currentItemRigidBody;
    Collider currentItemCollider;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isHandsFree = itemContainer.transform.childCount == 0;
    }

    void Update()
    {
        Look();
        Move();
        Jump();
        Gravity();
        Interactions();
        DropCurrentItem();
    }

    void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, maxLookUp, maxLookDown);

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    void Move()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");

        Vector3 moveAmount = transform.right * xMovement + transform.forward * zMovement;
        playerController.Move(moveAmount * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    void Gravity()
    {
        isGrounded = Physics.CheckSphere(groundCheckPoint.position, checkSphereRadius, groundLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = groundedDownVelocity;
        }

        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    void Interactions()
    {
        Vector3 castOriginPlace = playerCamera.transform.position + playerCamera.transform.forward * sphereCastDeviation;
        if (Physics.SphereCast(castOriginPlace, sphereCastRadius, playerCamera.transform.forward, out objectHit, sphereCastRange))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (objectHit.collider.gameObject.TryGetComponent(out IInteractable interactable))
                {
                    interactable.PlayerInteracted();
                }
            }
        }
    }

    public Vector3 GetHitPoint()
    {
        return objectHit.point;
    }

    public Transform GetCurrentItem()
    {
        currentItem = itemContainer.transform.GetChild(0);
        return currentItem;
    }

    void DropCurrentItem()
    {
        if (Input.GetKey(KeyCode.Q) && !isHandsFree)
        {
            currentItem = GetCurrentItem();
            currentItemRigidBody = currentItem.GetComponent<Rigidbody>();
            currentItemCollider = currentItem.GetComponent<Collider>();

            currentItemRigidBody.isKinematic = false;
            currentItemCollider.isTrigger = false;
            currentItem.SetParent(null);
            currentItemRigidBody.linearVelocity = playerController.velocity;
            currentItemRigidBody.AddForce(playerCamera.transform.forward * dropForwardForce, ForceMode.Impulse);
            currentItemRigidBody.AddForce(playerCamera.transform.up * dropUpwardForce, ForceMode.Impulse);

            isHandsFree = true;
        }
    }

    void OnDrawGizmos()
    {
        if (playerCamera == null) return;

        Gizmos.color = Color.green;
        Vector3 castOriginPlace = playerCamera.transform.position + playerCamera.transform.forward * sphereCastDeviation;

        Gizmos.DrawWireSphere(castOriginPlace, sphereCastRadius);
        Gizmos.DrawWireSphere(castOriginPlace + playerCamera.transform.forward * sphereCastRange, sphereCastRadius);
    }
}