using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ItemList
{
    public GameObject finalCombinedItem;
    public List<GameObject> requiredItems;
}

public class RequiredItemsChecker : MonoBehaviour, IInteractable
{
    [SerializeField] Transform itemSpots;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] List<ItemList> itemsRecipies;

    Transform[] itemsPosition;
    List<string> itemsNeeded = new List<string>();
    List<string> itemsDeposited = new List<string>();
    Player player;
    Vector3 closestPosition;
    bool closestPositionIsOccupied;
    float checkSphereRadius = 0.2f;
    int currentRecipeIndex = -1;

    void Awake()
    {
        itemsPosition = new Transform[itemSpots.childCount];
        for (int i = 0; i < itemSpots.childCount; i++)
        {
            itemsPosition[i] = itemSpots.GetChild(i);
        }

        UpdateItemsNeeded();
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

                player.isHandsFree = true;
                Invoke("CheckIfCanCombineItems", 0.1f);
            }
        }
    }

    public void UpdateItemsNeeded()
    {
        if (currentRecipeIndex < itemsRecipies.Count - 1)
        {
            currentRecipeIndex++;
        }
        else
        {
            return;
        }

        ItemList currentRecipe = itemsRecipies[currentRecipeIndex];
        itemsNeeded.Clear();
        foreach (var item in currentRecipe.requiredItems)
        {
            itemsNeeded.Add(item.name);
        }
        itemsNeeded.Sort();
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

        for (int i = 0; i < itemsPosition.Length; i++)
        {
            Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
            if (itemInSphere != null)
            {
                itemsDeposited.Add(itemInSphere.gameObject.name.Replace("(Clone)", ""));
            }
        }

        if (itemsDeposited.Count != itemsNeeded.Count)
        {
            Debug.Log("Failed. Not enough items");
            return;
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

        if (TryGetComponent(out ICompletionHandler completionHandler))
        {
            Debug.Log("Success. Items combined");
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

    public GameObject GetFinalCombinedItem()
    {
        return itemsRecipies[currentRecipeIndex].finalCombinedItem;
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