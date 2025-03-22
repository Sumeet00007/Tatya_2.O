using UnityEngine;

public interface IInteractable
{
    void PlayerInteracted();
}

public interface ICompletionHandler
{
    void OnCompletion();
}