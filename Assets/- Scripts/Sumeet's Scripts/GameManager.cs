using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern for easy access
    public GameObject gameOverImage; // Reference to the Game Over UI Image
    public float gameOverDelay = 0.5f;
    //variable to play GameOverCutscene
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        if (gameOverImage != null)
        {
           gameOverImage.SetActive(false);
        }
    }

    public void ShowGameOver()
    {
        if (gameOverImage != null)
        {
            Invoke(nameof(DisplayGameOver), gameOverDelay);
        }
    }

    public void DisplayGameOver()
    {
        gameOverImage.SetActive(true);
        Invoke(nameof(RestartLevel),1f);
        //Play GameOver Cutscene

        //update CheckPoint
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
