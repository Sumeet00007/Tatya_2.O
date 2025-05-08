using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour, IInteractable
{
    public bool canPickUp = true;
    public DoorOpening door;
    public AudioSource itemSound;

    [SerializeField] Vector3 itemPositionDeviation = new Vector3(0, 0, 0);
    [SerializeField] Vector3 itemRotationDeviation = new Vector3(0, 0, 0);

    private AudioSource audioSource;
    private Rigidbody rb;
    private Collider coll;
    private Player player;

    //private int originalLayer;
    //private int tLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        player = FindFirstObjectByType<Player>();

        // Cache layer indices
        //originalLayer = LayerMask.NameToLayer("Items");
        //tLayer = LayerMask.NameToLayer("Tlayer");
    }

    public void PlayerInteracted()
    {
        if (gameObject.CompareTag("WrongDoll"))
        {
            Invoke(nameof(TriggerGameOver), 0.5f);
        }

        if (gameObject.CompareTag("DoorKey"))
        {
            if (door != null)
            {
                door.hasKey = true;
            }
            else
            {
                return;
            }
        }

        if (player.isHandsFree && canPickUp)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            transform.SetParent(player.itemContainer);
            transform.localPosition = itemPositionDeviation;
            transform.localRotation = Quaternion.Euler(0, 0, 0) * Quaternion.Euler(itemRotationDeviation);

            //SetLayerRecursively(gameObject, tLayer); // Change layer for item and all children
            Debug.Log("Changed Layer to Tlayer");

            player.isHandsFree = false;
            itemSound.Play();
        }
    }

    public void DropItem()
    {
        transform.SetParent(null);
        rb.isKinematic = false;
        coll.isTrigger = false;

       // SetLayerRecursively(gameObject, originalLayer); // Revert layer for item and all children
        //Debug.Log("Changed Layer back to Items");

        player.isHandsFree = true;
    }

    //private void SetLayerRecursively(GameObject obj, int newLayer)
    //{
    //    obj.layer = newLayer;
    //    foreach (Transform child in obj.transform)
    //    {
    //        SetLayerRecursively(child.gameObject, newLayer);
    //    }
    //}

    private void TriggerGameOver()
    {
        GameManager.Instance.ShowGameOverImage();
    }
}
