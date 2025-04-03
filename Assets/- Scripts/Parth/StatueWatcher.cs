using UnityEngine;

public class StatueRotate : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float playerCheckRadius;
    [SerializeField] float yDeviation;
    [SerializeField] float rotationSpeed;

    Renderer statueRenderer;
    AudioSource audioSource;
    float distanceToPlayer;

    void Awake()
    {
        statueRenderer = GetComponent<Renderer>();
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        audioSource.loop = true;
    }

    void Update()
    {
        distanceToPlayer = Vector3.Distance(player.position, transform.position);
        if (!statueRenderer.isVisible && distanceToPlayer < playerCheckRadius && Mathf.Abs(player.position.y - transform.position.y) < 5f)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            targetRotation *= Quaternion.Euler(0, yDeviation, 0);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            if (Quaternion.Angle(transform.rotation, targetRotation) > 3f)
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.UnPause();
                }
            }
            else
            {
                if (audioSource.isPlaying)
                {
                    audioSource.Pause();
                }
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, playerCheckRadius);
    }
}