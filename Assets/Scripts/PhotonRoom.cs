using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    public int multiplayscene;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
        SceneManager.sceneLoaded += onscenefinishloading;                                                                                                                                                                                                                                                                  
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
        SceneManager.sceneLoaded -= onscenefinishloading;

    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("in a room");
        startgame();
    
    }
    void startgame()
    {
        if (!PhotonNetwork.IsMasterClient) { return; }

        PhotonNetwork.LoadLevel(multiplayscene);
    }
    void onscenefinishloading(Scene scene,LoadSceneMode mode)
    {
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel(0);
    }

}
