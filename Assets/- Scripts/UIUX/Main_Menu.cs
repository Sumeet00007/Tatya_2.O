using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
    void Update()
    {
        LockCursor(false);
    }
    public void PlayGame()
    {

        SceneManager.LoadScene("StartingCutscene");
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

    private void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}