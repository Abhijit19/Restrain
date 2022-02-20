using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyPlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetupPlayer();
        }
    }

    private void SetupPlayer()
    {

        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;

        string operatorName = "DefaultPlayer";
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey(Constants.PLAYERKEYS.OPERATOR))
        {
            operatorName = (string)PhotonNetwork.LocalPlayer.CustomProperties[Constants.PLAYERKEYS.OPERATOR];
            Debug.LogWarning($"Selected Operator {operatorName}");
        }

        GameObject player = PhotonNetwork.Instantiate($"PhotonPrefabs/{operatorName}", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)
    }
}
