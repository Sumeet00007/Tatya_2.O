using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    public void PlayGame()
    {

        SceneManager.LoadScene("Post Processing test 1");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Debug.Log("Game quit hoya Guru!");
        Application.Quit();
    }
    public void Back()
    {
        SceneManager.LoadScene("Main Menu");
    }
}