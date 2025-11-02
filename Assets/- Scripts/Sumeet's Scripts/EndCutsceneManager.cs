using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class EndCutsceneManager : MonoBehaviour
{
    public GameObject gameOverScreenIMG;

    private void Awake()
    {
        gameOverScreenIMG.SetActive(false);
        StartCoroutine(OnGameOverScreen());
    }
  

   IEnumerator OnGameOverScreen()
   {
        //wait for cutscene duration
        yield return new WaitForSeconds(165.0f);
        gameOverScreenIMG.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Main Menu");
   }
}
