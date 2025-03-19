using UnityEngine;

public class HeadBobbing : MonoBehaviour
{
    [Header("Bobbing Settings")]
    public float bobSpeed = 7f;            // Slightly faster for exaggerated effect
    public float verticalBobAmount = 0.1f; // More vertical bobbing
    public float horizontalBobAmount = 0.05f; // Slight horizontal sway
    public CharacterController playerController;

    private Vector3 originalLocalPos;
    private float timer;

    private void Start()
    {
        originalLocalPos = transform.localPosition;
    }

    private void Update()
    {
        if (playerController == null) return;

        if (IsPlayerMoving())
        {
            timer += Time.deltaTime * bobSpeed;

            float bobOffsetY = Mathf.Sin(timer) * verticalBobAmount;
            float bobOffsetX = Mathf.Sin(timer * 0.5f) * horizontalBobAmount; // Slow horizontal sway

            Vector3 newPos = new Vector3(originalLocalPos.x + bobOffsetX,
                                        originalLocalPos.y + bobOffsetY,
                                        originalLocalPos.z);

            transform.localPosition = Vector3.Lerp(transform.localPosition, newPos, Time.deltaTime * 10f);
        }
        else
        {
            // Reset smoothly when idle
            timer = 0;
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalLocalPos, Time.deltaTime * 6f);
        }
    }

    private bool IsPlayerMoving()
    {
        return playerController.isGrounded &&
               (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f);
    }
}
