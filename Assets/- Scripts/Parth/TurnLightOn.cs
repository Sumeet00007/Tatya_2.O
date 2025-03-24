using UnityEngine;

public class TurnLightOn : MonoBehaviour, ICompletionHandler
{
    [SerializeField] Light pointLight;
    public AudioSource switchONSound;

    void Start()
    {
        pointLight.enabled = false;
    }

    public void OnCompletion()
    {
        switchONSound.Play();   
        Debug.Log("Correct Items");
        pointLight.enabled = true;
    }
}
