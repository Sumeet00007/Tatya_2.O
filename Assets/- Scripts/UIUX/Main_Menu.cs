using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Balle Balle Paaji Game shuru Hoya!");
        //SceneManager.LoadScene("Stage 1");
    }

    public void Settings()
    {
        Debug.Log("Kuch nahi Paaji Settings mai");
    }

    public void QuitGame()
    {
        Debug.Log("Game quit hoya Guru!");
        Application.Quit();
    }

    //public void Back()
    //{
    //    SceneManager.LoadScene("Main Menu");
    //}

    //public void Credits()
    //{
    //    //SceneManager.LoadScene("Credits");
    //}
}