using UnityEngine;

public class CanPickUpTransister : MonoBehaviour, ICompletionHandler
{
    [SerializeField] GameObject transister;

    public void OnCompletion(Transform[] itemsPosition, LayerMask itemLayerMask, float checkSphereRadius)
    {
        transister.GetComponent<Items>().canPickUp = true;
    }
}
