using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ItemCombiner : MonoBehaviour, IInteractable
{
    [SerializeField] Transform[] itemsPosition;
    [SerializeField] Transform finalItemPosition;
    [SerializeField] GameObject finalItemPrefab;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] List<string> itemsNeeded;
    [SerializeField] float checkSphereRadius = 0.2f;

    List<string> itemsDeposited;
    Player player;
    Transform item;
    Rigidbody itemRB;
    Collider itemColl;
    Vector3 closestPosition;
    bool closestPositionIsOccupied;

    void Start()
    {
        itemsNeeded.Sort();
        itemsDeposited = new List<string>();
        player = FindFirstObjectByType<Player>();
    }

    public void PlayerInteracted()
    {
        //Debug.Log("Press E to deposite " + objectHit.transform.name);
        //Add UI prompt "Press e to deposite _itemName." instead of debug.....
        if (!player.isHandsFree)
        {
            GetUnoccupiedPlace();
            if (!closestPositionIsOccupied)
            {
                item = player.GetCurrentItem();
                itemRB = item.GetComponent<Rigidbody>();
                itemColl = item.GetComponent<Collider>();

                itemRB.isKinematic = false;
                itemColl.isTrigger = false;
                item.transform.SetParent(null);
                item.transform.position = closestPosition;
                item.transform.localRotation = Quaternion.identity;

                player.isHandsFree = true;
                Invoke("CheckIfCanCombineItems", 0.1f);
            }
        }
    }

    void GetUnoccupiedPlace()
    {
        float smallestDistance = Mathf.Infinity;
        for (int i = 0; i < itemsPosition.Length; i++)
        {
            float currentPointDistance = Vector3.Distance(itemsPosition[i].position, player.GetHitPoint());
            if (currentPointDistance < smallestDistance)
            {
                smallestDistance = currentPointDistance;
                closestPosition = itemsPosition[i].position;
            }
        }

        closestPositionIsOccupied = Physics.CheckSphere(closestPosition, checkSphereRadius, itemLayerMask);
    }

    void CheckIfCanCombineItems()
    {
        itemsDeposited.Clear();

        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
            if (itemInSphere != null)
            {
                Debug.Log("3");
                itemsDeposited.Add(itemInSphere.gameObject.name);
            }
            else
            {
                Debug.Log("4");
                return;
            }
        }

        itemsDeposited.Sort();

        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            if (itemsDeposited[i] != itemsNeeded[i])
            {
                Debug.Log("Failed. Not correct items");
                return;
            }
        }

        CombineItems();
    }

    void CombineItems()
    {
        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
            if (itemInSphere != null)
            {
                Destroy(itemInSphere.gameObject);
            }
        }

        Instantiate(finalItemPrefab, finalItemPosition.position, Quaternion.identity);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (itemsPosition != null)
        {
            foreach (var pos in itemsPosition)
            {
                Gizmos.DrawWireSphere(pos.position, checkSphereRadius);
            }
        }
    }
}