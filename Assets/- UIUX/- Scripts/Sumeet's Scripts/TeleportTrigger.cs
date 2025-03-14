using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    public Transform targetPos;
    public Image blackScreen;
    public float fadeDuration = 3f;


    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            CharacterController controller = other.GetComponent<CharacterController>();

            if (controller != null && targetPos != null)
            {
                StartCoroutine(FadeIn(other.transform, controller, targetPos));

            }
            else
            {
                // Debug.LogWarning("Level2 StartPos is not assigned or CharacterController is missing!");
            }
        }

    }

    public IEnumerator FadeIn(Transform pos, CharacterController ch, Transform level2Pos)
    {
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / fadeDuration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            ch.enabled = false;
            pos.position = level2Pos.position;
            ch.enabled = true;
            yield return null;
        }
        blackScreen.color = new Color(0, 0, 0, 0); // Fully transparent
    }


}
