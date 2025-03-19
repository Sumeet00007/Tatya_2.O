using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FootstepSoundManager : MonoBehaviour
{
    [Header("Footstep Settings")]
    public AudioClip[] footstepClips;
    public float stepInterval = 0.5f;
    [Range(0f, 1f)]
    public float footstepVolume = 1f;
    public float volumeMultiplier = 2f;

    [Header("Player Settings")]
    public float movementThreshold = 0.1f;

    private AudioSource audioSource;
    private CharacterController characterController;
    private float stepTimer;
    private bool wasWalkingLastFrame = false; // Track state to detect start of walking

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        bool isWalking = IsPlayerWalking();

        if (isWalking)
        {
            // If just started walking, play the footstep instantly
            if (!wasWalkingLastFrame)
            {
                PlayFootstep();
                stepTimer = 0f; // Reset timer after the instant step
            }

            stepTimer += Time.deltaTime;
            if (stepTimer >= stepInterval)
            {
                PlayFootstep();
                stepTimer = 0f;
            }
        }
        else
        {
            stepTimer = 0f; // Reset timer if player stops
        }

        wasWalkingLastFrame = isWalking;
    }

    private bool IsPlayerWalking()
    {
        return characterController.isGrounded &&
              (Mathf.Abs(Input.GetAxis("Horizontal")) > movementThreshold ||
               Mathf.Abs(Input.GetAxis("Vertical")) > movementThreshold);
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0) return;

        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.PlayOneShot(clip, footstepVolume * volumeMultiplier);
    }
}
