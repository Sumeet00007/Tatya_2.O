using UnityEngine;
using System.Collections;

public class PatienceMusicManager : MonoBehaviour
{
    [Header("Patience Bar Reference")]
    public PatienceBar patienceBar;

    [Header("Audio Sources")]
    public AudioSource primarySource;
    public AudioSource secondarySource;

    [Header("Audio Clips")]
    public AudioClip leastTenseMusic;
    public AudioClip moreTenseMusic;
    public AudioClip mostTenseMusic;
    public AudioClip finalOverMusic;

    [Header("Settings")]
    public float transitionDuration = 2f; // Time taken for smooth transitions

    private AudioClip currentClip;
    private bool isPrimaryActive = true;

    void Start()
    {
        if (patienceBar == null || primarySource == null || secondarySource == null)
        {
            Debug.LogError("Missing required components! Assign in Inspector.");
            return;
        }

        // Initialize both AudioSources
        primarySource.loop = true;
        secondarySource.loop = true;

        // Start with least tense music as the default background music
        PlayMusic(leastTenseMusic, true);
    }

    void Update()
    {
        UpdateMusicBasedOnPatience();
    }

    void UpdateMusicBasedOnPatience()
    {
        float patienceLevel = patienceBar.currentPatience / patienceBar.maxPatienceTime;

        AudioClip targetClip;

        if (!patienceBar.patiencebarmanager.activeSelf)
        {
            // If patience bar is disabled, always play least tense music
            targetClip = leastTenseMusic;
        }
        else if (patienceLevel <= 0.3f)
            targetClip = leastTenseMusic;
        else if (patienceLevel <= 0.6f)
            targetClip = moreTenseMusic;
        else if (patienceLevel < 1f)
            targetClip = mostTenseMusic;
        else
            targetClip = finalOverMusic;

        if (targetClip != currentClip)
        {
            TransitionToMusic(targetClip);
        }
    }

    void TransitionToMusic(AudioClip newClip)
    {
        if (newClip == null || newClip == currentClip)
            return;

        currentClip = newClip;

        // Swap active AudioSource for crossfade effect
        if (isPrimaryActive)
        {
            StartCoroutine(Crossfade(primarySource, secondarySource, newClip));
        }
        else
        {
            StartCoroutine(Crossfade(secondarySource, primarySource, newClip));
        }

        isPrimaryActive = !isPrimaryActive;
    }

    IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, AudioClip newClip)
    {
        float elapsedTime = 0f;

        // Set up new clip on secondary source
        toSource.clip = newClip;
        toSource.volume = 0f;
        toSource.Play();

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / transitionDuration;

            fromSource.volume = Mathf.Lerp(1f, 0f, progress);
            toSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        fromSource.Stop();
    }

    void PlayMusic(AudioClip clip, bool immediateStart = false)
    {
        if (primarySource.isPlaying)
            primarySource.Stop();

        if (secondarySource.isPlaying)
            secondarySource.Stop();

        primarySource.clip = clip;
        primarySource.volume = 1f;
        primarySource.Play();

        currentClip = clip;
        isPrimaryActive = true;
    }
}
