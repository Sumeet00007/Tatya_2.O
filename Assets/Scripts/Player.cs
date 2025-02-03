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

    [SerializeField] Transform itemContainer;
    [SerializeField] float sphereCastRadius;
    [SerializeField] float sphereCastRange;
    [SerializeField] float sphereCastDeviation;
    [SerializeField] float dropForwardForce;
    [SerializeField] float dropUpwardForce;

    RaycastHit objectHit;
    Transform currentItem;
    Rigidbody currentItemRigidBody;
    Collider currentItemCollider;
    DepositeSite depositeSite;
    bool isHandsFree;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
        isHandsFree = itemContainer.transform.childCount == 0;
        Vector3 castOriginPlace = playerCamera.transform.position + playerCamera.transform.forward * sphereCastDeviation;
        if (Physics.SphereCast(castOriginPlace, sphereCastRadius, playerCamera.transform.forward, out objectHit, sphereCastRange))
        {
            //Debug.Log(objectHit.transform.name);
            if (objectHit.collider.CompareTag("Item") && isHandsFree)
            {
                CollectItem();
            }

            if (objectHit.collider.CompareTag("Deposite Site") && !isHandsFree)
            {
                DepositeItem();
            }
        }
    }

    void CollectItem()
    {
        //Debug.Log("Press E to collect " + objectHit.transform.name);
        //Add UI prompt "Press e to collect _itemName." instead of debug.....
        if (Input.GetKeyDown(KeyCode.E))
        {
            objectHit.rigidbody.isKinematic = true;
            objectHit.collider.isTrigger = true;
            objectHit.transform.SetParent(itemContainer);
            objectHit.transform.localPosition = Vector3.zero;
            objectHit.transform.localRotation = Quaternion.identity;

            currentItem = itemContainer.transform.GetChild(0);
            currentItemRigidBody = currentItem.GetComponent<Rigidbody>();
            currentItemCollider = currentItem.GetComponent<Collider>();
        }
    }

    void DepositeItem()
    {
        Debug.Log("Press E to deposite " + objectHit.transform.name);
        //Add UI prompt "Press e to deposite _itemName." instead of debug.....
        if (Input.GetKeyDown(KeyCode.E))
        {
            depositeSite = objectHit.transform.GetComponent<DepositeSite>();
            currentItemRigidBody.isKinematic = false;
            currentItemCollider.isTrigger = false;
            currentItem.transform.SetParent(null);
            currentItem.transform.position = depositeSite.GetUnoccupiedPlace();
            currentItem.transform.localRotation = Quaternion.identity;
        }
    }

    void DropCurrentItem()
    {
        if (Input.GetKey(KeyCode.Q) && !isHandsFree)
        {
            currentItemRigidBody.isKinematic = false;
            currentItemCollider.isTrigger = false;
            currentItem.transform.SetParent(null);
            currentItemRigidBody.linearVelocity = playerController.velocity;
            currentItemRigidBody.AddForce(playerCamera.transform.forward * dropForwardForce, ForceMode.Impulse);
            currentItemRigidBody.AddForce(playerCamera.transform.up * dropUpwardForce, ForceMode.Impulse);
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