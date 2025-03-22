using UnityEngine;

public class DebugCorrectItems : MonoBehaviour, ICompletionHandler
{
    public void OnCompletion()
    {
        Debug.Log("Correct Items");
    }
}