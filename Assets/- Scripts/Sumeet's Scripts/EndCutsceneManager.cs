using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        yield return new WaitForSeconds(45.0f);
        gameOverScreenIMG.SetActive(true);
   }
}
