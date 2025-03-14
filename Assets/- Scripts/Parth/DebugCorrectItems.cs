using UnityEngine;

public class DebugCorrectItems : MonoBehaviour, ICompletionHandler
{
    public void OnCompletion(Transform[] itemsPosition, LayerMask itemLayerMask, float checkSphereRadius)
    {
        Debug.Log("Correct Items");
    }
}