using UnityEngine;

[DisallowMultipleComponent]
public class TransistorDepositor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform itemContainer;
    [SerializeField] LayerMask depositLayerMask;
    [SerializeField] float sphereCastRange = 2f;
    [SerializeField] float sphereCastRadius = 0.5f;

    private Transform currentItem;
    private Rigidbody currentItemRigidBody;
    private Collider currentItemCollider;

    void Update()
    {
        DepositTransistor();
    }

    void DepositTransistor()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentItem != null)
        {
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out hit, sphereCastRange, depositLayerMask))
            {
                TransistorPuzzleManager puzzleManager = hit.transform.GetComponent<TransistorPuzzleManager>();
                if (puzzleManager != null)
                {
                    Transistor transistor = currentItem.GetComponent<Transistor>();
                    if (transistor != null)
                    {
                        Vector3 placePosition = puzzleManager.GetUnoccupiedPlace();
                        if (placePosition != Vector3.zero)
                        {
                            currentItemRigidBody.isKinematic = false;
                            currentItemCollider.isTrigger = false;
                            currentItem.SetParent(null);
                            currentItem.position = placePosition;
                            currentItem.localRotation = Quaternion.identity;

                            puzzleManager.RegisterTransistorPlacement(transistor);

                            // Clear the reference
                            currentItem = null;
                        }
                    }
                }
            }
        }
    }

    public void HoldItem(Transform item)
    {
        currentItem = item;
        currentItemRigidBody = item.GetComponent<Rigidbody>();
        currentItemCollider = item.GetComponent<Collider>();
    }
}
