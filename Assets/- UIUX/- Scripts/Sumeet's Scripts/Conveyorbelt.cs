using UnityEngine;

public class Conveyorbelt : MonoBehaviour
{
    public GameObject belt;
    public Transform endpoint;
    public float speed;

    void OnCollisionStay(Collision other)
    {
        other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
    }
}