using UnityEngine;

public class TurnLightOn : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Light pointLight;

    void Start()
    {
        pointLight.enabled = false;
    }

    public void OnCompletion(Transform[] itemsPosition, LayerMask itemLayerMask, float checkSphereRadius)
    {
        Debug.Log("Correct Items");
        pointLight.enabled = true;
    }
}
