using UnityEngine;

public class TVJumpScare : MonoBehaviour
{
    public GameObject zombieArm;
    public AudioSource zombieScare;
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
            //Debug.Log("Player Interacted");

         zombieScare.Play();


            zombieArm.SetActive(true);
            Invoke(nameof(DestroyGameObject), 1.5f);
        }
    }

    public void DestroyGameObject()
    {
        Destroy(zombieArm.gameObject);
    }
}
