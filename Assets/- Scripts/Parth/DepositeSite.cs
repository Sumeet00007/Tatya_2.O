using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class DepositeSite : MonoBehaviour, IInteractable
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
    bool[] isSlotFilled;

    void Start()
    {
        itemsNeeded.Sort();
        isSlotFilled = new bool[itemsPosition.Length];
        itemsDeposited = new List<string>();
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        CheckIfCanCombineItems();
    }

    void CheckIfCanCombineItems()
    {
        itemsDeposited.Clear();

        for (int i = 0; i < itemsNeeded.Count; i++)
        {
            isSlotFilled[i] = Physics.CheckSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask);
            if (isSlotFilled[i])
            {
                Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
                if (itemInSphere != null)
                {
                    itemsDeposited.Add(itemInSphere.gameObject.name);
                }
            }
            else
            {
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

        CombineItem();
    }

    void CombineItem()
    {
        Debug.Log("Combine Item.");
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

    public Vector3 GetUnoccupiedPlace()
    {
        for (int i = 0; i < itemsPosition.Length; i++)
        {
            isSlotFilled[i] = Physics.CheckSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask);
            if (!isSlotFilled[i])
            {
                return itemsPosition[i].position;
            }
        }

        return Vector3.zero;
    }

    public void PlayerInteracted()
    {
        //Debug.Log("Press E to deposite " + objectHit.transform.name);
        //Add UI prompt "Press e to deposite _itemName." instead of debug.....
        if (!player.isHandsFree)
        {
            item = player.GetCurrentItem();
            itemRB = item.GetComponent<Rigidbody>();
            itemColl = item.GetComponent<Collider>();

            itemRB.isKinematic = false;
            itemColl.isTrigger = false;
            item.transform.SetParent(null);
            item.transform.position = GetUnoccupiedPlace();
            item.transform.localRotation = Quaternion.identity;

            player.isHandsFree = true;
        }
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
