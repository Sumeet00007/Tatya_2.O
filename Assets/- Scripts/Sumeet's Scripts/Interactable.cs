using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract  class Interactable : MonoBehaviour
{
   
   
    
    //this fuction will be called from player.
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        //we won't have to write any code in this function it's just template fuction for others
        
    }
}
