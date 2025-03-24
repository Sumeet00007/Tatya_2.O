using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// [System.Serializable]
// public class ItemList
// {
//     public string listName; // Helps identify lists in Inspector
//     public List<GameObject> requiredItems;
// }

public class Testing : MonoBehaviour//, IInteractable
{
    [SerializeField] Transform itemSpots;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] List<ItemList> itemLists; // Multiple Lists in Inspector
    [SerializeField] int currentListIndex; // Select Active List

    // Transform[] itemsPosition;
    // List<string> itemsNeeded;
    // List<string> itemsDeposited;
    // Player player;
    // Vector3 closestPosition;
    // bool closestPositionIsOccupied;
    // float checkSphereRadius = 0.2f;
}

// void Start()
// {
//     itemsPosition = new Transform[itemSpots.childCount];
//     for (int i = 0; i < itemSpots.childCount; i++)
//     {
//         itemsPosition[i] = itemSpots.GetChild(i);
//     }

//     player = FindFirstObjectByType<Player>();
//     SetCurrentList(currentListIndex); // Load Initial List
// }

// public void SetCurrentList(int index)
// {
//     if (index < 0 || index >= itemLists.Count) return;

//     currentListIndex = index;
//     itemsNeeded = itemLists[index].requiredItems.Select(item => item.name).ToList();
//     itemsNeeded.Sort();
//     itemsDeposited = new List<string>();
// }

// public void PlayerInteracted()
// {
//     if (!player.isHandsFree)
//     {
//         GetUnoccupiedPlace();
//         if (!closestPositionIsOccupied)
//         {
//             Transform currentItem = player.GetCurrentItem();
//             Rigidbody currentItemRB = currentItem.GetComponent<Rigidbody>();
//             Collider currentItemColl = currentItem.GetComponent<Collider>();

//             currentItemRB.isKinematic = false;
//             currentItemColl.isTrigger = false;
//             currentItem.transform.SetParent(null);
//             currentItem.transform.position = closestPosition;
//             currentItem.transform.localRotation = Quaternion.identity;

//             player.isHandsFree = true;
//             Invoke("CheckIfCanCombineItems", 0.1f);
//         }
//     }
// }

// void GetUnoccupiedPlace()
// {
//     float smallestDistance = Mathf.Infinity;
//     for (int i = 0; i < itemsPosition.Length; i++)
//     {
//         float currentPointDistance = Vector3.Distance(itemsPosition[i].position, player.GetHitPoint());
//         if (currentPointDistance < smallestDistance)
//         {
//             smallestDistance = currentPointDistance;
//             closestPosition = itemsPosition[i].position;
//         }
//     }

//     closestPositionIsOccupied = Physics.CheckSphere(closestPosition, checkSphereRadius, itemLayerMask);
// }

// void CheckIfCanCombineItems()
// {
//     itemsDeposited.Clear();

//     for (int i = 0; i < itemsNeeded.Count; i++)
//     {
//         Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
//         if (itemInSphere != null)
//         {
//             itemsDeposited.Add(itemInSphere.gameObject.name.Replace("(Clone)", ""));
//         }
//         else
//         {
//             return;
//         }
//     }

//     itemsDeposited.Sort();

//     for (int i = 0; i < itemsNeeded.Count; i++)
//     {
//         if (itemsDeposited[i] != itemsNeeded[i])
//         {
//             return;
//         }
//     }

//         if (TryGetComponent(out ICompletionHandler completionHandler))
//         {
//             completionHandler.OnCompletion();
//         }
//     }

//     public void ChangeListFromAnotherScript(int newIndex)
// {
//     SetCurrentList(newIndex);
// }

//     public Transform[] GetItemsPosition() => itemsPosition;
//     public float GetCheckSphereRadius() => checkSphereRadius;
//     public LayerMask GetItemLayerMask() => itemLayerMask;

//     void OnDrawGizmos()
//     {
//         Gizmos.color = Color.red;
//         if (itemsPosition != null)
//         {
//             foreach (var pos in itemsPosition)
//             {
//                 Gizmos.DrawWireSphere(pos.position, checkSphereRadius);
//             }
//         }
//     }
// }
