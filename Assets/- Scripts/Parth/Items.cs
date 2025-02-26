using UnityEngine;

public class Items : MonoBehaviour, IInteractable
{
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
        if (player.isHandsFree)
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