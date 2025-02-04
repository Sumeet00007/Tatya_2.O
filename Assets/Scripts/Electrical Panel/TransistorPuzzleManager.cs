using UnityEngine;

[DisallowMultipleComponent]
public class TransistorPuzzleManager : MonoBehaviour
{
    [Header("Transistor Compartment Positions")]
    [SerializeField] Transform[] transistorSlots; // Slots where transistors can be placed.

    [Header("Puzzle Object References")]
    [SerializeField] GameObject roomLight; // The room light to be activated.
    [SerializeField] GameObject blackObject; // The black object to change color.

    [Header("Detection Settings")]
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] float checkSphereRadius = 0.2f;

    private int placedTransistorCount = 0; // Tracks the number of transistors placed.

    void Start()
    {
        // Ensure initial states
        if (roomLight != null) roomLight.SetActive(false);
        if (blackObject != null) blackObject.GetComponent<Renderer>().material.color = Color.black;
    }

    public Vector3 GetUnoccupiedPlace()
    {
        foreach (Transform slot in transistorSlots)
        {
            bool isOccupied = Physics.CheckSphere(slot.position, checkSphereRadius, itemLayerMask);
            if (!isOccupied)
            {
                return slot.position;
            }
        }
        return Vector3.zero;
    }

    public void RegisterTransistorPlacement(Transistor transistor)
    {
        if (!transistor.isPlaced)
        {
            transistor.isPlaced = true;
            placedTransistorCount++;

            if (placedTransistorCount == transistorSlots.Length)
            {
                ActivateRoomLights();
            }
        }
    }

    void ActivateRoomLights()
    {
        Debug.Log("Puzzle Solved! Room Lights On.");
        if (roomLight != null) roomLight.SetActive(true);
        if (blackObject != null) blackObject.GetComponent<Renderer>().material.color = Color.white;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var pos in transistorSlots)
        {
            Gizmos.DrawWireSphere(pos.position, checkSphereRadius);
        }
    }
}
