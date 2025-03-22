using UnityEngine;

public class TVJumpScare : MonoBehaviour
{
    public GameObject zombieArm;
    void Awake()
    {
        zombieArm.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player Interacted");
            zombieArm.SetActive(true);
        }
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
