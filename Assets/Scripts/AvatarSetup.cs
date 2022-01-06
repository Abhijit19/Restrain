using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarSetup : MonoBehaviour
{
    PhotonView pv;
    public GameObject mycharacter;
    public int charvalue;
    int spawnpicker;
    void Start()
    {
         // Random.Range(0, GameSetup.GS.spawnpoints.Length);
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
           // pv.RPC("RPC_addchar", RpcTarget.AllBuffered,PlayerInfo.playerinfo.selectedCharacter);
        }
      
       
    }

  
    [PunRPC]
    void RPC_addchar(int whichchar)
    {
        mycharacter = Instantiate(PlayerInfo.playerinfo.allchars[whichchar],
            GameSetup.GS.spawnpoints[spawnpicker].transform.position,Quaternion.identity,transform);
        charvalue = whichchar;
        spawnpicker += 1;
        Debug.Log("1");
    }
}
