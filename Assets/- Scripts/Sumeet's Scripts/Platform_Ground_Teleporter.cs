using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class Platform_Ground_Teleporter : MonoBehaviour
{
    public Transform platformPuzzlePos;
    public Transform groundFloorPos;
    public Image blackScreen;
    public float fadeDuration = 3f;
    public bool firstTime = true;

    private void Start()
    {
        blackScreen.color = new Color(0, 0, 0, 0);

        if(platformPuzzlePos == null && groundFloorPos==null)
        {
            Debug.LogError(" transform are null");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterController controller = other.GetComponent<CharacterController>();

            //for final lighting scene
            //if (firstTime)
            //{
            //    StartCoroutine(FadeIn(other.transform, controller, platformPuzzlePos));
            //    firstTime = false;
            //}

            //for originalScene
            if (firstTime)
            {
                //StartCoroutine(FadeIn(other.transform, controller, platformPuzzlePos));
                firstTime = false;
            }

            else
            {
                StartCoroutine(FadeIn(other.transform, controller, groundFloorPos));
            }

        }
    }

    public IEnumerator FadeIn(Transform pos, CharacterController ch, Transform target)
    {
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / fadeDuration;
            blackScreen.color = new Color(0, 0, 0, alpha);
            ch.enabled = false;
            pos.position = target.position;
            ch.enabled = true;
            yield return null;
        }

       

        blackScreen.color = new Color(0, 0, 0, 0); // Fully transparent
    }
}
