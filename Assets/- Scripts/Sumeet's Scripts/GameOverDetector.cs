using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameOverDetector : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Invoke(nameof(TriggerGameOver), 0.5f);
        }
    }

    private void TriggerGameOver()
    {
        GameManager.Instance.ShowGameOverPlatformPuzzle();
    }


}
