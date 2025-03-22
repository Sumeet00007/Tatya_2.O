using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
   
    public Animator anim;

    public void OpenDoor()
    {
        anim.SetTrigger("openDoor");
    }



}
