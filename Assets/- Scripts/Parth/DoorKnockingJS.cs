using UnityEngine;

public class DoorKnockingJS : MonoBehaviour
{
    [SerializeField] GameObject haldi;
    [SerializeField] float knockSoundDelay = 0.5f;
    [SerializeField] Transform player;

    Vector3 haldiOriginalPosition;
    AudioSource audioSource;
    MeshRenderer meshRenderer;
    bool startKnocking = false;
    bool jumpScare = false;
    float timer;

    void Start()
    {
        haldiOriginalPosition = haldi.transform.position;
        audioSource = GetComponent<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void Update()
    {
        if (haldi.transform.position != haldiOriginalPosition && !startKnocking)
        {
            startKnocking = true;
            meshRenderer.enabled = true;
        }

        if (startKnocking)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                audioSource.Play();
                timer = knockSoundDelay;
            }

            Vector3 direction = player.position - transform.position;
            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit))
            {
                if (hit.transform == player && !jumpScare)
                {
                    Debug.Log("jumpscare Player");
                }
            }
        }

        if (Vector3.Distance(transform.position, player.position) > 50f)
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, 50f);
    }
}