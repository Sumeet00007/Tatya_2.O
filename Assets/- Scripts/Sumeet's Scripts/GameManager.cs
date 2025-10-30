using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern for easy access
    public GameObject gameOverImage; // Reference to the Game Over UI Image
    public float gameOverDelayforCutscene = 0.5f;
    //variable to play GameOverCutscene
    public GameObject gameOverCutScene;
    public float gameOverDelayforImg;
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
        if (gameOverImage != null)
        {
            gameOverImage.SetActive(false);
        }

       gameOverCutScene.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverImage != null)
        {
            Invoke(nameof(DisplayGameOver), gameOverDelayforCutscene);
        }
    }

    public void DisplayGameOver()
    {
        Invoke(nameof(RestartLevel), 4.5f);
        //Play GameOver Cutscene
        gameOverCutScene.SetActive(true);
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

    public void ShowGameOverImage()
    {
        gameOverImage.SetActive(true);
        Invoke(nameof(RestartLevel), gameOverDelayforImg);
    }
}
