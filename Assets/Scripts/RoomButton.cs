using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour
{
    public Text nametxt,sizetxt;


    public string room_Name;
    public int room_siZe;

    public void setroom()
    {
        nametxt.text=room_Name;
        sizetxt.text = room_siZe.ToString();
    }
    public void onclickjoinroom()
    {
        PhotonNetwork.JoinRoom(room_Name); 
    }
}
