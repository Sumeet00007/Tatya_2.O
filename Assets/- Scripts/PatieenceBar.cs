using UnityEngine;
using UnityEngine.UI;

public class PatienceBar : MonoBehaviour
{
    [Header("Patience Settings")]
    public float maxPatienceTime = 10f;
    public float currentPatience = 0f;
    public GameObject player;

    [Header("UI Settings")]
    public Image patienceBar;

    private bool isPlayerAlive = true;
    public GameObject patiencebarmanager;

    [Header("Heartbeat Settings")]
    public AudioSource heartbeatSource;
    public float minHeartbeatPitch = 0.8f;
    public float maxHeartbeatPitch = 2f;
    public float maxHeartbeatVolume = 1f;
    public float heartbeatFadeSpeed = 1f;

    [Header("Horror Overlay Settings")]
    public CanvasGroup horrorOverlay;
    public float maxOverlayAlpha = 0.8f;
    public float maxBlinkSpeed = 6f; // Pulse rate at max intensity

    private float baseBlinkSpeed = 1f;
    private bool heartbeatActive = false; // 🔹 Starts off

    void Start()
    {
        ResetPatienceBar();
        DisablePatienceBar();
    }

    void Update()
    {
        if (isPlayerAlive)
        {
            IncreasePatience(Time.deltaTime);
        }

        UpdateEffects();
    }

    public void ResetPatienceBar()
    {
        currentPatience = 0f;
        isPlayerAlive = true;
        UpdatePatienceBar();
        ResetEffects();
    }

    public void IncreasePatience(float amount)
    {
        currentPatience += amount;
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime + 5);
        UpdatePatienceBar();

        if (currentPatience >= maxPatienceTime)
        {
            KillPlayer();
        }
    }

    public void DecreasePatience(float percentage)
    {
        if (!isPlayerAlive) return;

        float decreaseAmount = (percentage / 100f) * maxPatienceTime;
        currentPatience -= decreaseAmount;
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime + 5);
        UpdatePatienceBar();
    }

    void UpdatePatienceBar()
    {
        if (patienceBar != null)
            patienceBar.fillAmount = currentPatience / maxPatienceTime;
    }

    public void KillPlayer()
    {
        if (isPlayerAlive)
        {
            Debug.Log("Player has died due to enemy’s patience running out!");
            isPlayerAlive = false;
            GameManager.Instance.ShowGameOverPatienceEmpty();
            Invoke(nameof(ZeroFill), 1.5f);
        }
    }

    private void ZeroFill()
    {
        currentPatience = 0;
        UpdatePatienceBar();
    }

    public void SetMaxPatienceTime(float newMaxTime)
    {
        maxPatienceTime = Mathf.Max(1f, newMaxTime);
        currentPatience = Mathf.Clamp(currentPatience, 0, maxPatienceTime + 5);
        ResetPatienceBar();
    }

    public void InsertdeltaTime()
    {
        IncreasePatience(Time.deltaTime);
    }

    public void EnablePatienceBar()
    {
        patiencebarmanager.SetActive(true);
        EnableEffects();
    }

    public void DisablePatienceBar()
    {
        patiencebarmanager.SetActive(false);
        DisableEffects();
    }

    // ---------------- HORROR EFFECT SYSTEM ----------------

    void UpdateEffects()
    {
        if (heartbeatSource == null && horrorOverlay == null) return;

        float fillPercent = Mathf.Clamp01(currentPatience / maxPatienceTime);

        // 🔹 Start effects only when patience crosses 50%
        if (fillPercent >= 0.5f && !heartbeatActive)
        {
            heartbeatActive = true;
            EnableEffects();
        }
        else if (fillPercent < 0.5f && heartbeatActive)
        {
            heartbeatActive = false;
            DisableEffects();
        }

        if (!heartbeatActive) return;

        // Map 50%–100% range to 0–1 intensity
        float intensity = Mathf.InverseLerp(0.5f, 1f, fillPercent);

        // --- Heartbeat ---
        if (heartbeatSource != null)
        {
            float targetVolume = Mathf.Lerp(0f, maxHeartbeatVolume, intensity);
            float targetPitch = Mathf.Lerp(minHeartbeatPitch, maxHeartbeatPitch, intensity);

            heartbeatSource.volume = Mathf.MoveTowards(heartbeatSource.volume, targetVolume, Time.deltaTime * heartbeatFadeSpeed);
            heartbeatSource.pitch = Mathf.MoveTowards(heartbeatSource.pitch, targetPitch, Time.deltaTime * heartbeatFadeSpeed);
        }

        // --- Horror Overlay ---
        if (horrorOverlay != null)
        {
            float targetAlpha = Mathf.Lerp(0f, maxOverlayAlpha, intensity);
            float blinkSpeed = Mathf.Lerp(baseBlinkSpeed, maxBlinkSpeed, intensity);
            float blink = (Mathf.Sin(Time.time * blinkSpeed) + 1f) / 2f;

            float finalAlpha = targetAlpha * blink;
            horrorOverlay.alpha = Mathf.MoveTowards(horrorOverlay.alpha, finalAlpha, Time.deltaTime * 5f);
        }
    }

    void EnableEffects()
    {
        if (heartbeatSource != null)
        {
            heartbeatSource.volume = 0f;
            heartbeatSource.pitch = minHeartbeatPitch;
            if (!heartbeatSource.isPlaying) heartbeatSource.Play();
        }

        if (horrorOverlay != null)
        {
            horrorOverlay.alpha = 0f;
            horrorOverlay.gameObject.SetActive(true);
        }
    }

    void DisableEffects()
    {
        if (heartbeatSource != null)
            heartbeatSource.Stop();

        if (horrorOverlay != null)
        {
            horrorOverlay.alpha = 0f;
            horrorOverlay.gameObject.SetActive(false);
        }
    }

    void ResetEffects()
    {
        heartbeatActive = false;

        if (heartbeatSource != null)
        {
            heartbeatSource.volume = 0f;
            heartbeatSource.pitch = minHeartbeatPitch;
            heartbeatSource.Stop();
        }

        if (horrorOverlay != null)
        {
            horrorOverlay.alpha = 0f;
            horrorOverlay.gameObject.SetActive(false);
        }
    }
}
