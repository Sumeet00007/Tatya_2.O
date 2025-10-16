using System.Collections;
using UnityEngine;

public class WrongPlatformDestroyer : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 4f; // Time before platform gets destroyed
    [SerializeField] private float shakeDuration = 2f; // Duration for Right Platform
    [SerializeField] private float shakeIntensity = 0.2f; // Adjust intensity
    private bool isPlayerOnPlatform = false;
    private float timer = 0f;
    private Transform cameraTransform;
    private Vector3 originalCameraLocalPosition;
    private Coroutine shakeCoroutine;
    //For folly sound effect

    public AudioSource crumblesoundFx;
    private Vector3 initialPosition;
    //public float platformResetTime = 3.0f;

    private void Start()
    {
        cameraTransform = Camera.main.transform; // Get main camera
        originalCameraLocalPosition = cameraTransform.localPosition; // Store local position
        initialPosition= transform.localPosition;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Item"))
        {
            if (CompareTag("wrongPlatform") ) 
            {
                if (!isPlayerOnPlatform)
                {
                    crumblesoundFx.Play();
                    isPlayerOnPlatform = true;
                    timer = 0f;
                    shakeCoroutine = StartCoroutine(ShakeCamera());
                    Invoke(nameof(StopCameraShake), 0.3f);
                }

                timer += Time.deltaTime;

                if (timer >= destroyDelay)
                {
                    gameObject.SetActive(false);
                    //Invoke(nameof(Reset), platformResetTime);
                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && CompareTag("RightPlatform"))
        {
            shakeCoroutine = StartCoroutine(ShakeCamera());
            Invoke(nameof(StopCameraShake), 0.3f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerOnPlatform = false;
            timer = 0f;

            cameraTransform.localPosition = originalCameraLocalPosition; // Reset camera
        }
    }

    private IEnumerator ShakeCamera()
    {
        float elapsedtime = 0f;
        while (elapsedtime < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            cameraTransform.localPosition = originalCameraLocalPosition + new Vector3(x, y, 0);
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        cameraTransform.localPosition = originalCameraLocalPosition; // Reset after shake
    }

    private void StopCameraShake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine); // Stop the specific coroutine
            shakeCoroutine = null;
        }
        cameraTransform.localPosition = originalCameraLocalPosition; // Reset position
    }

    //Function to appear platform again.
    //private void Reset()
    //{
    //    transform.position= initialPosition;
    //    gameObject.SetActive(true);
    //}
}
