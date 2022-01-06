using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayRoom : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Text>().text = PhotonNetwork.CurrentRoom.Name;
    }
}
