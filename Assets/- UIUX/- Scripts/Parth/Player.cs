using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Diagnostics;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [Header("Mouse Look Variables")]

    [SerializeField] Camera playerCamera;
    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float maxLookUp, maxLookDown;

    Vector2 lookInput;
    float cameraXRotation = 0f;
    float playerYRotaion = 0f;

    [Header("Movement Variables")]

    [SerializeField] CharacterController playerController;
    [SerializeField] float moveSpeed = 5f;

    Vector2 moveInput;

    [Header("Jumping Variables")]

    [SerializeField] float jumpHeight;

    [Header("Gravity Variables")]

    [SerializeField] float gravity = -9.81f;
    [SerializeField] float groundedDownVelocity = -2f;

    Vector3 velocity;

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
        Cursor.visible = false;
        isHandsFree = itemContainer.transform.childCount == 0;
    }

    void Update()
    {
        Look();
        Move();
        Gravity();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void Look()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;
        playerYRotaion = mouseX;
        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, maxLookUp, maxLookDown);

        transform.Rotate(Vector3.up * mouseX);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraXRotation, 0f, 0f);
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Move()
    {
        Vector3 moveAmount = transform.right * moveInput.x + transform.forward * moveInput.y;
        playerController.Move(moveAmount * moveSpeed * Time.deltaTime);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && playerController.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }
    }

    void Gravity()
    {
        if (playerController.isGrounded && velocity.y < 0)
        {
            velocity.y = groundedDownVelocity;
        }

        velocity.y += gravity * Time.deltaTime;
        playerController.Move(velocity * Time.deltaTime);
    }

    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            Interactions();
        }
    }

    void Interactions()
    {
        Vector3 castOriginPlace = playerCamera.transform.position + playerCamera.transform.forward * sphereCastDeviation;
        if (Physics.SphereCast(castOriginPlace, sphereCastRadius, playerCamera.transform.forward, out objectHit, sphereCastRange))
        {
            if (objectHit.collider.gameObject.TryGetComponent(out IInteractable interactable))
            {
                interactable.PlayerInteracted();



            }
        }
    }

    void OnDrop(InputValue value)
    {
        if (value.isPressed)
        {
            DropCurrentItem();
        }
    }

    void DropCurrentItem()
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

    public Vector3 GetHitPoint()
    {
        return objectHit.point;
    }

    public Transform GetCurrentItem()
    {
        currentItem = itemContainer.transform.GetChild(0);
        return currentItem;
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