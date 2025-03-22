using UnityEngine;

public class Trashbin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Item"))
        {
           Destroy(other.gameObject);
        }
    }
}
