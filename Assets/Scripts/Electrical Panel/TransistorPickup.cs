using UnityEngine;

[DisallowMultipleComponent]
public class TransistorPickup : MonoBehaviour
{
    private bool isHeld = false;
    private Rigidbody rb;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    void Update()
    {
        if (isHeld && Input.GetKeyDown(KeyCode.Q)) // Drop Item
        {
            DropTransistor();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.E)) // Pick Up
        {
            TransistorDepositor depositor = other.GetComponent<TransistorDepositor>();
            if (depositor != null)
            {
                isHeld = true;
                rb.isKinematic = true;
                col.isTrigger = true;
                transform.SetParent(other.transform);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;

                depositor.HoldItem(transform);
            }
        }
    }

    void DropTransistor()
    {
        isHeld = false;
        rb.isKinematic = false;
        col.isTrigger = false;
        transform.SetParent(null);
    }
}
