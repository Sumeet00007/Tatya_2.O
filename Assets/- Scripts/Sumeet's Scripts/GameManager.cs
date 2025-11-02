using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern for easy access
    public GameObject patiencegameOverImage; // Reference to the Game Over UI Image
    public GameObject platformgameOverImage;
    public float gameOverDelayforCutscene = 0.5f;
    //variable to play GameOverCutscene
    public GameObject gameOverCutScene;
  
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (gameOverCutScene == null)
        {
            Debug.LogError("gameOverCutscene is null");
        }
    }

    void Start()
    {
        if (patiencegameOverImage != null)
        {
            patiencegameOverImage.SetActive(false);
        }

        if(platformgameOverImage != null)
        {
            platformgameOverImage.SetActive(false);
        }

       gameOverCutScene.SetActive(false);
    }

    public void ShowGameOverPatienceEmpty()
    {
        if (patiencegameOverImage != null)
        {
            Invoke(nameof(DisplayGameOver), gameOverDelayforCutscene);
        }
    }

    public void DisplayGameOver()
    {
        
        //Play GameOver Cutscene
        gameOverCutScene.SetActive(true);
        StartCoroutine(PatienceEmptyImage());
        //update CheckPoint
        
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOverCutscene()
    {
        SceneManager.LoadScene("Ending CutScene");
    }

    IEnumerator EndGameOverScreen()
    {
        yield return new WaitForSeconds(4.1f);
        gameOverCutScene.SetActive(false);
    }

    public void ShowGameOverPlatformPuzzle()
    {
       
        gameOverCutScene.SetActive(true);
        StartCoroutine(PlatformDieImage());

    }

    IEnumerator PatienceEmptyImage()
    {
        yield return new WaitForSeconds(8.1f);
        patiencegameOverImage.SetActive(true) ;
        gameOverCutScene.SetActive(false);
        Invoke(nameof(RestartLevel), 2.0f);
    }

    IEnumerator PlatformDieImage()
    {
        yield return new WaitForSeconds(8.1f);
        platformgameOverImage.SetActive(true);
        gameOverCutScene.SetActive(false);
        Invoke(nameof(RestartLevel), 2.0f);

    }
}
