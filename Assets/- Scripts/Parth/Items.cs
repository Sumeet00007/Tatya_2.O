using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour, IInteractable
{
    public bool canPickUp = true;
    public DoorOpening door;
    public AudioSource itemSound;

    [SerializeField] Vector3 itemPositionDeviation = new Vector3(0, 0, 0);
    [SerializeField] Vector3 itemRotationDeviation = new Vector3(0, 0, 0);

    AudioSource audioSource;
    Rigidbody rb;
    Collider coll;
    Player player;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        player = FindFirstObjectByType<Player>();
       
    }

    public void PlayerInteracted()
    {
        //Debug.Log("Press E to collect " + objectHit.transform.name);
        //Add UI prompt "Press e to collect _itemName." instead of debug.....

        if (gameObject.CompareTag("WrongDoll")) // Replace "Collectible" with your actual tag
        {
            //Debug.Log("This is wrong Doll");
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
            player.isHandsFree = false;
           itemSound.Play();
        }
    }

    private void TriggerGameOver()
    {
        GameManager.Instance.ShowGameOverImage();
    }
}