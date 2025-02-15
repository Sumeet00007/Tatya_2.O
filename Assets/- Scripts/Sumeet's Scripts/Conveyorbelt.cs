using UnityEngine;

public class Conveyorbelt : MonoBehaviour
{
    public GameObject belt;
    public Transform endpoint;
    public float speed;

    private void OnTriggerStay(Collider other)
    {
        other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
    }

}
