using System.Linq;
using UnityEngine;

public class CombineItems : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Transform finalItemPosition;
    [SerializeField] GameObject finalItemPrefab;

    public void OnCompletion(Transform[] itemsPosition, LayerMask itemLayerMask, float checkSphereRadius)
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