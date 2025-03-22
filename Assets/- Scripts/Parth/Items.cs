using UnityEngine;
using UnityEngine.UI;

public class Items : MonoBehaviour, IInteractable
{
    public bool canPickUp = true;


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
            Debug.Log("This is wrong Doll");
            GameManager.Instance.ShowGameOver();
        }

        if (player.isHandsFree && canPickUp)
        {
            rb.isKinematic = true;
            coll.isTrigger = true;
            transform.SetParent(player.itemContainer);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            player.isHandsFree = false;
        }
    }
}