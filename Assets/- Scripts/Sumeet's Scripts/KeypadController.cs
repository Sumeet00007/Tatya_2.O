﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadController : MonoBehaviour
{
    public DoorController door;
    public string password;
    public int passwordLimit;
    public TMP_Text passwordText;
    public AudioSource passwordRight;
    public AudioSource passwordWrong;

    //[Header("Audio")]
    //public AudioSource audioSource;
    //public AudioClip correctSound;
    //public AudioClip wrongSound;

    private void Start()
    {
        passwordText.text = "";
    }

    public void PasswordEntry(string number)
    {
        if (number == "Clear")
        {
            Clear();
            return;
        }
        else if (number == "Enter")
        {
            Enter();
            return;
        }

        int length = passwordText.text.ToString().Length;
        if (length < passwordLimit)
        {
            passwordText.text = passwordText.text + number;
        }
    }

    public void Clear()
    {
        passwordText.text = "";
        passwordText.color = Color.white;
    }

    private void Enter()
    {
        if (passwordText.text == password)
        {
            door.OpenDoor();

            //if (audioSource != null)
            //    audioSource.PlayOneShot(correctSound);

            passwordText.color = Color.green;
            passwordRight.Play();
            StartCoroutine(waitAndClear());
        }
        else
        {
            //if (audioSource != null)
            //    audioSource.PlayOneShot(wrongSound);

            passwordText.color = Color.red;
            passwordWrong.Play();
            StartCoroutine(waitAndClear());
        }
    }

    IEnumerator waitAndClear()
    {

        yield return new WaitForSeconds(0.75f);
        Clear();
    }

}
