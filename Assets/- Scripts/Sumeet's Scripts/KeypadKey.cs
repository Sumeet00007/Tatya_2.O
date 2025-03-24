using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeypadKey : MonoBehaviour, IInteractable
{
    public string key;
    public AudioSource click;

    public void PlayerInteracted()
    {
        this.transform.GetComponentInParent<KeypadController>().PasswordEntry(key);
        click.Play();
    }
}
