using UnityEngine;

public class UItrigger : MonoBehaviour
{
    public GameObject ui_Notification;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
            ui_Notification.SetActive(true);
        }
    }
}
