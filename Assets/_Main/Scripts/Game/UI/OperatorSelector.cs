using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class OperatorSelector : MonoBehaviour
{
    private IEnumerator Start()
    {
        do
        {
            Debug.Log("Waiting for connection!");
            yield return null;
        } while (!PhotonNetwork.IsConnectedAndReady);

        //Set player operator on start
        string operatorName = "AnimatedPlayerTest";//PlayerPrefs.GetString(Constants.PLAYERKEYS.OPERATOR, "AnimatedPlayerTest");           //MOD
        OnOperatorSelected(operatorName);
    }

    public void OnOperatorSelected(string operatorName)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            //Set custom variable
            Hashtable hash = new Hashtable();
            hash.Add(Constants.PLAYERKEYS.OPERATOR, operatorName);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            //Save locally
            PlayerPrefs.SetString(Constants.PLAYERKEYS.OPERATOR, operatorName);
            Debug.Log("Player Operator Set :"+operatorName);
        }
    }
}
