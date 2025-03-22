using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // Singleton pattern for easy access
    public GameObject gameOverImage; // Reference to the Game Over UI Image

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
            Invoke(nameof(DisplayGameOver), 3.0f);
        }
    }

    public void DisplayGameOver()
    {
        gameOverImage.SetActive(true);
        //Play GameOver Cutscene

        //update CheckPoint
    }
}
