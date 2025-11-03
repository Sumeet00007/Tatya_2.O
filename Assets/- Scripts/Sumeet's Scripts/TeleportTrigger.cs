using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TeleportTrigger : MonoBehaviour
{
    [Header("Teleport Settings")]
    public Transform targetPos;
    public Vector3 targetRotation; // ⬅️ Add this to set facing direction (in degrees)

    [Header("Fade Settings")]
    public Image blackScreen;
    public float fadeDuration = 3f;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null && targetPos != null)
            {
                StartCoroutine(FadeIn(other.transform, controller, targetPos));
            }
        }
    }

    private IEnumerator FadeIn(Transform playerTransform, CharacterController controller, Transform level2Pos)
    {
        float timer = fadeDuration;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / fadeDuration;
            blackScreen.color = new Color(0, 0, 0, alpha);

            controller.enabled = false;
            playerTransform.position = level2Pos.position;
            playerTransform.rotation = Quaternion.Euler(targetRotation); // ⬅️ Apply rotation
            controller.enabled = true;

            yield return null;
        }

        blackScreen.color = new Color(0, 0, 0, 0); // Fade back to transparent
    }
}
