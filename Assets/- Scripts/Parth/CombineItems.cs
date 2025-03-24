using System.Linq;
using UnityEngine;

public class CombineItems : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Transform finalItemPosition;

    RequiredItemsChecker requiredItemsChecker;
    GameObject finalItemPrefab;
    Transform[] itemsPosition;
    LayerMask itemLayerMask;
    float checkSphereRadius = 0.2f;

    void Start()
    {
        requiredItemsChecker = GetComponent<RequiredItemsChecker>();
        itemsPosition = requiredItemsChecker.GetItemsPosition();
        itemLayerMask = requiredItemsChecker.GetItemLayerMask();
        checkSphereRadius = requiredItemsChecker.GetCheckSphereRadius();
    }

    public void OnCompletion()
    {
        for (int i = 0; i < itemsPosition.Length; i++)
        {
            Collider itemInSphere = Physics.OverlapSphere(itemsPosition[i].position, checkSphereRadius, itemLayerMask).FirstOrDefault();
            if (itemInSphere != null)
            {
                Destroy(itemInSphere.gameObject);
            }
        }

        finalItemPrefab = requiredItemsChecker.GetFinalCombinedItem();
        Instantiate(finalItemPrefab, finalItemPosition.position, Quaternion.identity);
        requiredItemsChecker.UpdateItemsNeeded();
    }
}