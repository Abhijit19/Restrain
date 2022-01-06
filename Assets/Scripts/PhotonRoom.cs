using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;


public class PhotonRoom : MonoBehaviourPunCallbacks,IInRoomCallbacks
{
    public static PhotonRoom photonRoom;
    PhotonView pv;
    public int multiplayscene;
    int currentscene;
    private void Awake()
    {
        if(PhotonRoom.photonRoom==null)
        {
            PhotonRoom.photonRoom = this;
        }
        else
        {
            if (PhotonRoom.photonRoom != this)
            {
                Destroy(PhotonRoom.photonRoom.gameObject);
                PhotonRoom.photonRoom = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

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
        currentscene = scene.buildIndex;
        if(currentscene == multiplayscene)
        {
            createplayer();
        }
    }
    void createplayer()
    {
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
            transform.position,
            Quaternion.identity);
    }

}
