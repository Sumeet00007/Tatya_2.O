using UnityEngine;

public class CanPickUpTransister : MonoBehaviour, ICompletionHandler
{
    [SerializeField] GameObject transister;

    public void OnCompletion()
    {
        transister.GetComponent<Items>().canPickUp = true;
    }
}
