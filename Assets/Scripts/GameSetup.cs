using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class GameSetup : MonoBehaviourPun
{
    public static GameSetup GS;
    public Transform[] spawnpoints;
    public GameObject[] characters;

    GameObject Player;
    private void OnEnable()
    {
        if(GameSetup.GS==null)
        {
            GameSetup.GS = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        int randomspawn = Random.Range(0, spawnpoints.Length);

        Player =  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs",characters[PlayerInfo.playerinfo.selectedCharacter].name),
            spawnpoints[randomspawn].position, Quaternion.identity,0, new object[] {photonView.ViewID}); 
    }


    public void KillPlayer()
    {
        PhotonNetwork.Destroy(Player);
    }
}
