using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhotonLobbyManager : MonoBehaviourPunCallbacks
{
    private string NickName = string.Empty;

    public void SetNickName(string nickName)
    {
        NickName = nickName;
    }
    public string GetNickName()
    {
        return NickName;
    }

    private void Connect()
    {
        //If already connected, return
        if (PhotonNetwork.IsConnected)
        { return; }

        //If nickname is not set, choose a random one
        if(NickName == string.Empty)
            NickName = "Player_"+Random.Range(10000, 99999);


        PhotonNetwork.NickName = NickName;
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }
}
