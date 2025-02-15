using UnityEngine;
using UnityEngine.SceneManagement;  // Add this to manage scene transitions

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField]
    private SoundLibrary sfxLibrary;
    [SerializeField]
    private AudioSource sfx2DSource;

    // Option to destroy the SoundManager on scene load
    public bool destroyOnNewScene = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
         
        }
        else
        {
            Instance = this;

            // If destroyOnNewScene is false, persist across scenes
            if (!destroyOnNewScene)
            {
                DontDestroyOnLoad(gameObject);
            }

            // Register to sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    // This method will be called every time a new scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (destroyOnNewScene)
        {
            // Destroy the SoundManager when a new scene loads
            Destroy(gameObject);
        }
    }

    public void PlaySound3D(AudioClip clip, Vector3 pos)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, pos);
        }
    }

    public void PlaySound3D(string soundName, Vector3 pos)
    {
        PlaySound3D(sfxLibrary.GetClipFromName(soundName), pos);
    }

    public void PlaySound2D(string soundName)
    {
        sfx2DSource.PlayOneShot(sfxLibrary.GetClipFromName(soundName));
    }

    private void OnDestroy()
    {
        // Unsubscribe from the sceneLoaded event when SoundManager is destroyed
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
