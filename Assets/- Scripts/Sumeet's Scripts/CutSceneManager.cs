using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class CutSceneManager : MonoBehaviour
{
   

    private void Start()
    {
        StartCoroutine(WelComeToMainLevel());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Main Final");
        }
    }


    IEnumerator WelComeToMainLevel()
    {
        yield return new WaitForSeconds(63.0f);
        //Load Main Scene
        SceneManager.LoadScene("Main Final");
    }

}
