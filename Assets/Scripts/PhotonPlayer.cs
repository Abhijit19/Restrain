using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using System.IO;

public class PhotonPlayer : MonoBehaviour
{
    PhotonView pv;
    public GameObject myavatar;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        int spawnpicker = Random.Range(0, GameSetup.GS.spawnpoints.Length);
        if (pv.IsMine)
        {
            myavatar = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerAvatar"),
            GameSetup.GS.spawnpoints[spawnpicker].position,
            GameSetup.GS.spawnpoints[spawnpicker].rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
