using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElectricPanelChecker : MonoBehaviour, IInteractable
{
    [SerializeField] Transform itemSpots;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] public GameObject[] itemsNeededGameObjects;
    public AudioSource transistorDepSound;
    Transform[] itemsPosition;
    List<string> itemsNeeded;
    List<string> itemsDeposited;
    Player player;
    Vector3 closestPosition;
    bool closestPositionIsOccupied;
    float checkSphereRadius = 0.2f;

    void Start()
    {
        itemsPosition = new Transform[itemSpots.childCount];
        for (int i = 0; i < itemSpots.childCount; i++)
        {
            itemsPosition[i] = itemSpots.GetChild(i);
        }

        itemsNeeded = new List<string>();
        foreach (var item in itemsNeededGameObjects)
        {
            itemsNeeded.Add(item.name);
        }

        itemsNeeded.Sort();
        itemsDeposited = new List<string>();
        player = FindFirstObjectByType<Player>();
    }

    public void PlayerInteracted()
    {
        if (!player.isHandsFree)
        {
            GetUnoccupiedPlace();
            if (!closestPositionIsOccupied)
            {
                Transform currentItem = player.GetCurrentItem();
                Rigidbody currentItemRB = currentItem.GetComponent<Rigidbody>();
                Collider currentItemColl = currentItem.GetComponent<Collider>();

                currentItemRB.isKinematic = false;
                currentItemColl.isTrigger = false;
                currentItem.transform.SetParent(null);
                currentItem.transform.position = closestPosition;
                currentItem.transform.localRotation = Quaternion.identity;

                transistorDepSound.Play();
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
                itemsDeposited.Add(itemInSphere.gameObject.name.Replace("(Clone)", ""));
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
                //Debug.Log("Failed. Not correct items");
                return;
            }
        }

        if (TryGetComponent(out ICompletionHandler completionHandler))
        {
            completionHandler.OnCompletion();
        }
    }

    public Transform[] GetItemsPosition()
    {
        return itemsPosition;
    }

    public float GetCheckSphereRadius()
    {
        return checkSphereRadius;
    }

    public LayerMask GetItemLayerMask()
    {
        return itemLayerMask;
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
