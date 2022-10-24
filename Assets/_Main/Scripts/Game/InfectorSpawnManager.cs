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
        return;
        if (PhotonNetwork.IsMasterClient)
        {
            Transform spawn = infectorPoint;
            Vector3 position = spawn.position;
            Quaternion rotation = spawn.rotation;
            string operatorName = "DefaultInfector";
            infector = PhotonNetwork.Instantiate($"Items/{operatorName}", position, rotation, 0);     // avoid this call on rejoin (ship was network instantiated before)
        }
    }

    public GameObject SpawnInfector()
    {
        Transform spawn = infectorPoint;
        Vector3 position = spawn.position;
        Quaternion rotation = spawn.rotation;
        string operatorName = "DefaultInfector";
        infector = PhotonNetwork.Instantiate($"Items/{operatorName}", position, rotation, 0);     // avoid this call on rejoin (ship was network instantiated before)
        return infector;
    }

    public void RemoveInfector()
    {
        if(infector != null)
        {
            Destroy(infector);
            infector = null;
        }
    }

    public void OnBeforeTransformParentChanged()
    {

    }
}
