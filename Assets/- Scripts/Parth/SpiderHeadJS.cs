using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
    [SerializeField] Renderer monsterRenderer;
    [SerializeField] Transform rayCastOrigin;
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float upwardForce = 5f;

    Rigidbody monsterRigidbody;
    Collider monsterCollider;
    Vector3 directionToCamera;
    float distance;
    bool haveJumped = false;

    void Start()
    {
        monsterRigidbody = GetComponent<Rigidbody>();
        monsterCollider = GetComponent<Collider>();
    }

    void Update()
    {
        directionToCamera = playerCamera.transform.position - rayCastOrigin.position;
        distance = directionToCamera.magnitude;

        if (Physics.Raycast(rayCastOrigin.position, directionToCamera.normalized, out RaycastHit hit, distance))
        {
            if (hit.transform.CompareTag("Player") && monsterRenderer.isVisible)
            {
                if (!haveJumped)
                {
                    monsterCollider.isTrigger = true;
                    Vector3 lookDirection = -playerCamera.transform.forward;
                    lookDirection.y = 0;
                    transform.rotation = Quaternion.LookRotation(lookDirection);
                    monsterRigidbody.AddForce(directionToCamera.normalized * jumpForce, ForceMode.Impulse);
                    monsterRigidbody.AddForce(Vector3.up * upwardForce, ForceMode.Impulse);
                    haveJumped = true;
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        if (rayCastOrigin != null && playerCamera != null)
        {
            Vector3 direction = playerCamera.transform.position - rayCastOrigin.position;
            float distance = direction.magnitude;
            direction.Normalize();

            Ray ray = new Ray(rayCastOrigin.position, direction);
            RaycastHit hit;

            Gizmos.color = Color.red;

            if (Physics.Raycast(ray, out hit, distance))
            {
                Gizmos.DrawLine(rayCastOrigin.position, hit.point);
                Gizmos.DrawSphere(hit.point, 0.05f);
            }
            else
            {
                Gizmos.DrawLine(rayCastOrigin.position, playerCamera.transform.position);
            }
        }
    }
}
