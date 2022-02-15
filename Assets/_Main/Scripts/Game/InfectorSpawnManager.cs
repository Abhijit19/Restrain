using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfectorSpawnManager : MonoBehaviour
{
    public Transform infectorPoint;
    public GameObject infector;
    public void OnGameStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Transform spawn = infectorPoint;
            Vector3 position = spawn.position;
            Quaternion rotation = spawn.rotation;
            string operatorName = "DefaultInfector";
            infector = PhotonNetwork.Instantiate($"Items/{operatorName}", position, rotation, 0);     // avoid this call on rejoin (ship was network instantiated before)
        }
    }
}
