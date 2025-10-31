using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("References")]
    public GameObject pauseMenu;
    public CanvasGroup gameCanvasGroup; // Attach your GameCanvas here
    public AudioSource dialogueAudioSource; // Attach the dialogue audio source

    [Header("Settings")]
    public float fadeDuration = 1f; // Fade-in speed when resuming

    public static bool isPaused;

    private MyGame.Dialogue.DialogueSystem dialogueSystem;
    private Coroutine fadeCoroutine;

    void Start()
    {
        pauseMenu.SetActive(false);
        if (gameCanvasGroup != null)
            gameCanvasGroup.alpha = 1f;

        dialogueSystem = FindFirstObjectByType<MyGame.Dialogue.DialogueSystem>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        // Instantly hide game canvas
        if (gameCanvasGroup != null)
            gameCanvasGroup.alpha = 0f;

        // Pause dialogue audio
        if (dialogueAudioSource != null && dialogueAudioSource.isPlaying)
            dialogueAudioSource.Pause();

        // Pause dialogue coroutine
        if (dialogueSystem != null)
            dialogueSystem.SetDialogueActive(false);

        LockCursor(false);
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        // Resume dialogue audio
        if (dialogueAudioSource != null)
            dialogueAudioSource.UnPause();

        // Resume dialogue coroutine
        if (dialogueSystem != null)
            dialogueSystem.SetDialogueActive(true);

        // Fade-in the game canvas
        if (gameCanvasGroup != null)
        {
            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);
            fadeCoroutine = StartCoroutine(FadeCanvas(gameCanvasGroup, 0f, 1f, fadeDuration));
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu G");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

    private IEnumerator FadeCanvas(CanvasGroup canvasGroup, float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        canvasGroup.alpha = startAlpha;

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime; // unaffected by timescale
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = endAlpha;
    }
}
