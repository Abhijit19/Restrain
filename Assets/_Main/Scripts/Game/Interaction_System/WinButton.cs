using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinButton : InteractableBase
{
    public override void OnInteract()
    {
        Debug.Log("INTERACTED: " + gameObject.name);
        //Add win logic here?
    }
}
