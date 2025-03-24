using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public AudioSource door;
    public Animator anim;

    public void OpenDoor()
    {
        door.Play();
        anim.SetTrigger("openDoor");
    }



}
