using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class REDemonDoll : MonoBehaviour
{
    //SerializeField] float gracePeriod = 0.5f;
    //[SerializeField] float jumpScareDistance = 2f;

    Player player;
    Animator anim;
    AudioSource audioSource;

    void Awake()
    {
        player = FindFirstObjectByType<Player>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //gameObject.SetActive(false);
    }

    void OnEnable()
    {
        audioSource.Play();
        StartCoroutine(CheckInputForLoss());
    }

    IEnumerator CheckInputForLoss()
    {
        if (Keyboard.current.anyKey.isPressed || Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            Debug.Log("Player Fucked Up.");
            JumpScare();
        }
        yield return null;
    }

    void JumpScare()
    {

    }
}