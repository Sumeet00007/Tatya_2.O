using UnityEngine;
using UnityEngine.Events;

public class LeachJumpscare : MonoBehaviour
{
    public UnityEvent onActionTriggered;
    


    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           // Debug.Log("Leach JumpScare Initiated")
;            TriggerEvent();
        }
    }
    public void TriggerEvent()
    {
        if (onActionTriggered != null)
        {
            onActionTriggered.Invoke();
          
        }
    }
}
