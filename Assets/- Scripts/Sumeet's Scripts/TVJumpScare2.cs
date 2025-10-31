using System.Collections;
using UnityEngine;

public class TVJumpScare2 : MonoBehaviour
{
    public GameObject colorVideo;
    public GameObject noiseEffect;
    public float videoDuration;

    private void Start()
    {
        colorVideo.SetActive(false);
        noiseEffect.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            colorVideo.SetActive(true);
            noiseEffect.SetActive(false);
            StartCoroutine(EnableNoiseEffect());
        }
       
    }

    IEnumerator EnableNoiseEffect()
    {
        yield return new WaitForSeconds(videoDuration);
        Destroy(colorVideo.gameObject);
        noiseEffect.SetActive(true);
        Destroy(this.gameObject);
    }
}
