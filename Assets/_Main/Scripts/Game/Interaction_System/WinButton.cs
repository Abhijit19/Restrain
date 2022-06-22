using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WinButton : InteractableBase
{
    public override void OnInteract()
    {
        Debug.Log("INTERACTED: " + gameObject.name);
        //Add win logic here?
        GameEndReason("Defender Won");
    }

    private void GameEndReason(string reason)
    {
        Hashtable props = new Hashtable
        {
            {"GameSatate", (int)2},
            {"Reason", reason}
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(props);
    }
}
