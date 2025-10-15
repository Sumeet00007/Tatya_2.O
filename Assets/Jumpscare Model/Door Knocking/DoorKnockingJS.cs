using UnityEngine;

public class DoorKnockingJS : MonoBehaviour
{
    [SerializeField] GameObject haldi;
    [SerializeField] float knockSoundDelay = 0.5f;
    [SerializeField] Transform player;
    [SerializeField] GameObject maleModel;
    [SerializeField] DoorOpener knockingDoor;
    [SerializeField] float playerOutOfRangeDistance = 50f;
    [SerializeField] float animationTime;

    Vector3 haldiOriginalPosition;
    AudioSource audioSource;
    bool startKnocking = false;
    bool jumpScare = false;
    bool destroyAfterAnimation = false;
    bool checkIfOutOfRange = false;
    float timer;

    void Start()
    {
        haldiOriginalPosition = haldi.transform.position;
        audioSource = GetComponent<AudioSource>();
        maleModel.SetActive(false);
    }

    void Update()
    {
        if (haldi.transform.position != haldiOriginalPosition && !startKnocking)
        {
            startKnocking = true;
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
            if (Physics.Raycast(transform.position, direction.normalized, out RaycastHit hit, playerOutOfRangeDistance))
            {
                if (hit.transform == player && !jumpScare)
                {
                    Debug.Log("jumpscare Player");
                    jumpScare = true;
                    maleModel.SetActive(true);
                    player.GetComponent<Player>().TurnOffFlashlight();
                    destroyAfterAnimation = true;
                    checkIfOutOfRange = true;
                }
            }
        }

        if (destroyAfterAnimation)
        {
            animationTime -= Time.deltaTime;
            if (animationTime <= 0f)
            {
                Destroy(gameObject);
            }
        }

        if (Vector3.Distance(transform.position, player.position) > playerOutOfRangeDistance && checkIfOutOfRange)
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