using System.Linq;
using UnityEngine;

public class CombineItems : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Transform finalItemPosition;
    [SerializeField] GameObject finalItemPrefab;

    RequiredItemsChecker requiredItemsChecker;
    [SerializeField] Transform[] itemsPosition;
    [SerializeField] LayerMask itemLayerMask;
    [SerializeField] float checkSphereRadius = 0.2f;

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

        Instantiate(finalItemPrefab, finalItemPosition.position, Quaternion.identity);
    }
}